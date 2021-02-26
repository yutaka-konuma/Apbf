// <copyright file="StorageSampleService.cs" company="Above Beyond Project Framework">
// Copyright (c) Above Beyond Project Framework. All rights reserved.
// </copyright>

namespace Sample04Storage.Service
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using AbpfUtil.Models;
    using AbpfUtil.Util;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Sample01Base.Service;

    public class StorageSampleService : SampleServiceBase
    {

        // コンテナ名称
        private const string containerName = "dvp-abp-framework-jpe-st-01"; // "linabpfprotohddblob";

        private ILoggerFactory loggerFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="StorageSampleService"/> class.
        /// </summary>
        /// <param name="logger">logger.</param>
        /// <param name="userInfo">userInfo.</param>
        public StorageSampleService(ILoggerFactory logger)
            : base(logger)
        {
            try
            {
                this.loggerFactory = logger;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public List<BlobData> GetStorageBlob(UserInfo userInfo)
        {
            // 指定コンテナの全ファイルの名称リスト取得
            return AbpfStorageUtil.GetStorageBlob(containerName, userInfo);
        }

        public int PutStorageBlob(IFormFile uploadFile, UserInfo userInfo)
        {
            // 指定コンテナにファイルを保存
            return AbpfStorageUtil.PutStorageBlob(containerName, Path.GetFileName(uploadFile.FileName), uploadFile, userInfo);
        }

        public Stream DownloadStorageBlob(string fileName, UserInfo userInfo)
        {
            // 指定コンテナのファイルを取得（ストリーム形式）
            return AbpfStorageUtil.GetDownloadBlobFileSteam(containerName, fileName, userInfo);
        }


    }
}
