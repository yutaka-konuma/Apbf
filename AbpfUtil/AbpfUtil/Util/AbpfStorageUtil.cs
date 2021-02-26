// <copyright file="AbpfStorageUtil.cs" company="Above Beyond Project Framework">
// Copyright (c) Above Beyond Project Framework. All rights reserved.
// </copyright>

namespace AbpfUtil.Util
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.CompilerServices;
    using AbpfUtil.Models;
    using Azure.Storage.Blobs;
    using Azure.Storage.Blobs.Models;
    using Azure.Storage.Blobs.Specialized;
    using Azure.Storage.Sas;
    using Microsoft.AspNetCore.Http;
    using Microsoft.WindowsAzure.Storage.Blob;

    /// <summary>
    /// ファイル送受信ユーティリティ(AzureStorage Container)
    /// </summary>
    public static class AbpfStorageUtil
    {
        /// <summary>
        /// 指定コンテナが存在するかチェック
        /// </summary>
        /// <param name="containerName">コンテナ名称</param>
        /// <param name="userInfo">ユーザー情報</param>
        /// <param name="memberName">メンバー名（呼び出し側は指定しない）</param>
        /// <param name="filePath">ファイル名（呼び出し側は指定しない）</param>
        /// <param name="lineNumber">行番号（呼び出し側は指定しない）</param>
        /// <returns>存在したらtrue</returns>
        public static BlobContainerClient GetCloudBlobClient(
            string containerName,
            UserInfo userInfo,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = -1)
        {
            // AzureStorageサービスリソースを取得
            BlobServiceClient blobServiceClient = new BlobServiceClient(AbpfConfigUtil.GetConnectionString("Storage"));
            if (blobServiceClient == null)
            {
                AbpfLogUtil.LogError(
                    $@"{containerName} ストレージにアクセスできません",
                    userInfo,
                    memberName,
                    filePath,
                    lineNumber);
                return null;
            }

            // 新しいBlobContainerClientオブジェクトを作成
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            if (containerClient == null)
            {
                AbpfLogUtil.LogError(
                    $@"{containerName} コンテナが見つかりません",
                    userInfo,
                    memberName,
                    filePath,
                    lineNumber);
                return null;
            }

            // 成功：True
            return containerClient;
        }

        /// <summary>
        /// 指定コンテナが存在するかチェック
        /// </summary>
        /// <param name="containerName">コンテナ名称</param>
        /// <param name="userInfo">ユーザー情報</param>
        /// <param name="memberName">メンバー名（呼び出し側は指定しない）</param>
        /// <param name="filePath">ファイル名（呼び出し側は指定しない）</param>
        /// <param name="lineNumber">行番号（呼び出し側は指定しない）</param>
        /// <returns>存在したらtrue</returns>
        public static bool ContainsContainer(
            string containerName,
            UserInfo userInfo,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = -1)
        {
            // AzureStorageサービスリソースを取得
            BlobServiceClient blobServiceClient = new BlobServiceClient(AbpfConfigUtil.GetConnectionString("Storage"));
            if (blobServiceClient == null)
            {
                AbpfLogUtil.LogError(
                    $@"{containerName} ストレージにアクセスできません",
                    userInfo,
                    memberName,
                    filePath,
                    lineNumber);
                return false;
            }

            // 新しいBlobContainerClientオブジェクトを作成
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            if (containerClient == null)
            {
                AbpfLogUtil.LogError(
                    $@"{containerName} コンテナが見つかりません",
                    userInfo,
                    memberName,
                    filePath,
                    lineNumber);
                return false;
            }

            // 成功：True
            return true;
        }

        /// <summary>
        /// 指定コンテナの全ファイルの名称リスト取得
        /// </summary>
        /// <param name="containerName">コンテナ名称</param>
        /// <param name="userInfo">ユーザー情報</param>
        /// <param name="memberName">メンバー名（呼び出し側は指定しない）</param>
        /// <param name="filePath">ファイル名（呼び出し側は指定しない）</param>
        /// <param name="lineNumber">行番号（呼び出し側は指定しない）</param>
        /// <returns>ファイル名称情報リスト</returns>
        public static List<BlobData> GetStorageBlob(
            string containerName,
            UserInfo userInfo,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = -1)
        {
            // AzureStorageサービスリソースを取得
            BlobServiceClient blobServiceClient = new BlobServiceClient(AbpfConfigUtil.GetConnectionString(AbpfConfigUtil.Storage));
            if (blobServiceClient == null)
            {
                AbpfLogUtil.LogError(
                    $@"{containerName} ストレージにアクセスできません",
                    userInfo,
                    memberName,
                    filePath,
                    lineNumber);
                return null;
            }

            // 新しいBlobContainerClientオブジェクトを作成
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            if (containerClient == null)
            {
                AbpfLogUtil.LogError(
                    $@"{containerName} コンテナが見つかりません",
                    userInfo,
                    memberName,
                    filePath,
                    lineNumber);
                return null;
            }

            // 全ファイルの名称リストを作成
            List<BlobData> blobDataList = new List<BlobData>();
            foreach (BlobItem blobItem in containerClient.GetBlobs())
            {
                blobDataList.Add(new BlobData
                {
                    FileName = blobItem.Name,
                    LastModified = blobItem.Properties.LastModified.Value.LocalDateTime,
                });
            }

            return blobDataList;
        }

        /// <summary>
        /// 指定コンテナにファイルを保存
        /// </summary>
        /// <param name="containerName">コンテナ名称</param>
        /// <param name="filename">ファイル名称</param>
        /// <param name="uploadFile">アップロードファイル</param>
        /// <param name="userInfo">ユーザー情報</param>
        /// <param name="memberName">メンバー名（呼び出し側は指定しない）</param>
        /// <param name="filePath">ファイル名（呼び出し側は指定しない）</param>
        /// <param name="lineNumber">行番号（呼び出し側は指定しない）</param>
        /// <returns>0:アップロード完了 -1:失敗 -9:コンテナ名称がない</returns>
        public static int PutStorageBlob(
            string containerName,
            string filename,
            IFormFile uploadFile,
            UserInfo userInfo,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = -1)
        {
            // AzureStorageサービスリソースを取得
            BlobServiceClient blobServiceClient = new BlobServiceClient(AbpfConfigUtil.GetConnectionString("Storage"));
            if (blobServiceClient == null)
            {
                AbpfLogUtil.LogError(
                    $@"{containerName} ストレージにアクセスできません",
                    userInfo,
                    memberName,
                    filePath,
                    lineNumber);
                return (int)AbpfConstants.AzureUploadResult.ContainerNotFound;
            }

            // 新しいBlobContainerClientオブジェクトを作成
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            if (containerClient == null)
            {
                AbpfLogUtil.LogError(
                    $@"{containerName} コンテナが見つかりません",
                    userInfo,
                    memberName,
                    filePath,
                    lineNumber);
                return (int)AbpfConstants.AzureUploadResult.ContainerNotFound;
            }

            // コンテナにファイルをアップロード
            if (uploadFile != null && uploadFile.Length > 0)
            {
                containerClient.UploadBlob(filename, uploadFile.OpenReadStream());
                AbpfLogUtil.LogDebug(
                    $@"{filename} ({uploadFile.ContentType}) - {uploadFile.Length} bytes アップロード完了",
                    userInfo,
                    memberName,
                    filePath,
                    lineNumber);
            }
            else
            {
                AbpfLogUtil.LogError(
                    $@"{containerName} ダウンロード失敗",
                    userInfo,
                    memberName,
                    filePath,
                    lineNumber);
                return (int)AbpfConstants.AzureUploadResult.Error;
            }

            // 成功
            return (int)AbpfConstants.AzureUploadResult.Success;
        }

        /// <summary>
        /// 指定コンテナの指定ファイルが存在するかチェック
        /// </summary>
        /// <param name="containerName">コンテナ名称</param>
        /// <param name="fileName">ファイル名</param>
        /// <param name="userInfo">ユーザー情報</param>
        /// <param name="memberName">メンバー名（呼び出し側は指定しない）</param>
        /// <param name="filePath">ファイル名（呼び出し側は指定しない）</param>
        /// <param name="lineNumber">行番号（呼び出し側は指定しない）</param>
        /// <returns>存在したらtrue</returns>
        public static bool ContainsBlobFile(
            string containerName,
            string fileName,
            UserInfo userInfo,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = -1)
        {
            // AzureStorageサービスリソースを取得
            BlobServiceClient blobServiceClient = new BlobServiceClient(AbpfConfigUtil.GetConnectionString("Storage"));
            if (blobServiceClient == null)
            {
                AbpfLogUtil.LogError(
                    $@"{containerName} ストレージにアクセスできません",
                    userInfo,
                    memberName,
                    filePath,
                    lineNumber);
                return false;
            }

            // 新しいBlobContainerClientオブジェクトを作成
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            if (containerClient == null)
            {
                AbpfLogUtil.LogError(
                    $@"{containerName} コンテナが見つかりません",
                    userInfo,
                    memberName,
                    filePath,
                    lineNumber);
                return false;
            }

            // 新しいBlobClientオブジェクトを作成
            BlobClient blobClient = containerClient.GetBlobClient(fileName);

            // ファイルが存在したらtrueを返す
            return blobClient != null;
        }

        /// <summary>
        /// 指定コンテナのファイルを取得（ストリーム形式）
        /// </summary>
        /// <param name="containerName">コンテナ名称</param>
        /// <param name="fileName">ダウンロードファイル名</param>
        /// <param name="userInfo">ユーザー情報</param>
        /// <param name="memberName">メンバー名（呼び出し側は指定しない）</param>
        /// <param name="filePath">ファイル名（呼び出し側は指定しない）</param>
        /// <param name="lineNumber">行番号（呼び出し側は指定しない）</param>
        /// <returns>Streamを返す</returns>
        public static Stream GetDownloadBlobFileSteam(
            string containerName,
            string fileName,
            UserInfo userInfo,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = -1)
        {
            // AzureStorageサービスリソースを取得
            BlobServiceClient blobServiceClient = new BlobServiceClient(AbpfConfigUtil.GetConnectionString("Storage"));
            if (blobServiceClient == null)
            {
                AbpfLogUtil.LogError(
                    $@"{containerName} ストレージにアクセスできません",
                    userInfo,
                    memberName,
                    filePath,
                    lineNumber);
                return null;
            }

            // 新しいBlobContainerClientオブジェクトを作成
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            if (containerClient == null)
            {
                AbpfLogUtil.LogError(
                    $@"{containerName} コンテナが見つかりません",
                    userInfo,
                    memberName,
                    filePath,
                    lineNumber);
                return null;
            }

            // ダウンロード用 URL取得テスト
            Uri uri = GetServiceSasUriForBlob(containerName, fileName, userInfo);
            Console.WriteLine(uri.ToString());

            // コンテナのファイルをダウンロードする
            return containerClient.GetBlobClient(fileName).OpenRead();
        }

        /// <summary>
        /// 指定コンテナのファイルのダウンロード用Uri取得
        /// </summary>
        /// <param name="containerName">コンテナ名称</param>
        /// <param name="fileName">ダウンロードファイル名</param>
        /// <param name="userInfo">ユーザー情報</param>
        /// <param name="memberName">メンバー名（呼び出し側は指定しない）</param>
        /// <param name="filePath">ファイル名（呼び出し側は指定しない）</param>
        /// <param name="lineNumber">行番号（呼び出し側は指定しない）</param>
        /// <returns>Uriを返す</returns>
        private static Uri GetServiceSasUriForBlob(
            string containerName,
            string fileName,
            UserInfo userInfo,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = -1)
        {
            // AzureStorageサービスリソースを取得
            BlobServiceClient blobServiceClient = new BlobServiceClient(AbpfConfigUtil.GetConnectionString("Storage"));
            if (blobServiceClient == null)
            {
                AbpfLogUtil.LogError(
                    $@"{containerName} ストレージにアクセスできません",
                    userInfo,
                    memberName,
                    filePath,
                    lineNumber);
                return null;
            }

            // 新しいBlobContainerClientオブジェクトを作成
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            if (containerClient == null)
            {
                AbpfLogUtil.LogError(
                    $@"{containerName} コンテナが見つかりません",
                    userInfo,
                    memberName,
                    filePath,
                    lineNumber);
                return null;
            }

            BlobClient blobClient = containerClient.GetBlobClient(fileName);

            // Check whether this BlobClient object has been authorized with Shared Key.
            if (blobClient.CanGenerateSasUri)
            {
                // Create a SAS token that's valid for one hour.
                BlobSasBuilder sasBuilder = new BlobSasBuilder()
                {
                    BlobContainerName = blobClient.GetParentBlobContainerClient().Name,
                    BlobName = blobClient.Name,
                    Resource = "b",
                };

                sasBuilder.ExpiresOn = DateTimeOffset.UtcNow.AddDays(7);
                sasBuilder.SetPermissions(BlobSasPermissions.Read);

                Uri sasUri = blobClient.GenerateSasUri(sasBuilder);
                Console.WriteLine("SAS URI for blob is: {0}", sasUri);
                Console.WriteLine();

                return sasUri;
            }
            else
            {
                Console.WriteLine(@"BlobClient must be authorized with Shared Key 
                          credentials to create a service SAS.");
                return null;
            }
        }
    }
}
