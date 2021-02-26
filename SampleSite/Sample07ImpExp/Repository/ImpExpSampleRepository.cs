// <copyright file="SearchSampleRepository.cs" company="Above Beyond Project Framework">
// Copyright (c) Above Beyond Project Framework. All rights reserved.
// </copyright>

namespace Search01Search.Repository
{
    using AbpfUtil.Models;
    using AbpfUtil.Util;
    using Azure.Storage.Blobs;
    using Azure.Storage.Blobs.Specialized;
    using GrapeCity.Documents.Excel;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Data.SqlClient;
    using OfficeOpenXml;
    using Sample01Base.Repository;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    /// <summary>
    /// 検索サンプルリポジトリ
    /// </summary>
    public class ImpExpSampleRepository : SampleRepositoryBase
    {
        private const string ContainerName = "dvp-abp-framework-jpe-st-temp-01";
        private const string DataSourceName = "lin_bpf_prot_hdd_temp_Storage";

        // SQLConnection
        private readonly SqlConnection con;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImpExpSampleRepository"/> class.
        /// </summary>
        /// <param name="con">DB接続</param>
        public ImpExpSampleRepository(SqlConnection con)
            : base(con)
        {
            this.con = con;
        }

        // ストレージのファイルに文字列を追記する関数
        private static void AppendText(AppendBlobClient appendBlob, string textToAppend)
        {
            appendBlob.AppendBlock(new MemoryStream(Encoding.UTF8.GetBytes(textToAppend)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        /// <param name="userInfo">ユーザー情報</param>
        /// <returns></returns>
        public Stream ExpCsv(string fileName, UserInfo userInfo)
        {
            List<SqlParameter> paramList = new List<SqlParameter>();

            StringBuilder sqlstr = new StringBuilder(@$"
SELECT PostCd  
      ,Country 
      ,City    
      ,Address 
FROM MPOST 
ORDER BY PostCd
            ");

            //container
            BlobContainerClient containerClient = AbpfStorageUtil.GetCloudBlobClient(ContainerName, userInfo);
            containerClient.CreateIfNotExists();
            var appendBlobClient = containerClient.GetAppendBlobClient(fileName);
            appendBlobClient.CreateIfNotExists();
            var appendBlobMaxAppendBlockBytes = appendBlobClient.AppendBlobMaxAppendBlockBytes;

            // Utf-8ファイルのBOMを出力しておく
            appendBlobClient.AppendBlock(new MemoryStream(new UTF8Encoding(true).GetPreamble()));

            // 1000行毎に出力する（メモリ消費を抑えつつ、出力速度を維持する為）
            StringBuilder sb = new StringBuilder();
            long cnt = 0;

            using SqlCommand com = new SqlCommand(sqlstr.ToString(), this.con);
            if (paramList.Count > 0)
            {
                com.Parameters.AddRange(paramList.ToArray());
            }

            using var sdr = com.ExecuteReader();
            if (sdr.HasRows == false)
            {
                throw new Exception("Data Not Found");
            }

            List<string> colList = new List<string>();
            while (sdr.Read())
            {
                if (cnt == 0)
                {
                    // ヘッダ行作成
                    for (var col = 0; col < sdr.FieldCount; col++)
                    {
                        colList.Add("\"" + sdr.GetName(col).Replace("\"", "\"\"") + "\""); // " を "" にエスケープ ＆ 前後に"を付与
                    }
                    sb.AppendLine(string.Join(",", colList));
                }

                colList.Clear();
                for (var col = 0; col < sdr.FieldCount; col++)
                {
                    colList.Add("\"" + sdr[col].ToString().Replace("\"", "\"\"") + "\""); // " を "" にエスケープ ＆ 前後に"を付与
                }
                sb.AppendLine(string.Join(",", colList));

                cnt++;

                if (cnt % 1000 == 0)
                {
                    AppendText(appendBlobClient, sb.ToString());
                    sb.Clear();
                }
            }

            if (sb.Length > 0)
            {
                AppendText(appendBlobClient, sb.ToString());
            }

            return containerClient.GetBlobClient(fileName).OpenRead(); 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        /// <param name="userInfo">ユーザー情報</param>
        /// <returns></returns>
        public Stream ExpXlsxDioDocs(string fileName, UserInfo userInfo)
        {
            List<SqlParameter> paramList = new List<SqlParameter>();

            StringBuilder sqlstr = new StringBuilder(@$"
SELECT PostCd  
      ,Country 
      ,City    
      ,Address 
FROM MPOST 
ORDER BY PostCd
            ");

            using (SqlCommand com = new SqlCommand(sqlstr.ToString(), this.con))
            {
                if (paramList.Count > 0)
                {
                    com.Parameters.AddRange(paramList.ToArray());
                }

                using (var sdr = com.ExecuteReader())
                {
                    if (sdr.HasRows == false)
                    {
                        throw new Exception("Data Not Found");
                    }

                    List<string[]> buffTable = new List<string[]>();

                    int rowCount = 0;
                    int columnCount = sdr.FieldCount;

                    // 新規ワークブックの作成
                    Workbook workbook = new Workbook();
                    IWorksheet worksheet = workbook.Worksheets[0];

                    string[] values = new string[sdr.FieldCount];
                    while (sdr.Read())
                    {
                        if (rowCount == 0)
                        {
                            // ヘッダ行作成
                            for (var col = 0; col < sdr.FieldCount; col++)
                            {
                                values[col] = sdr.GetName(col); 
                            }
                            buffTable.Add(values);
                            rowCount++;
                        }

                        values = new string[sdr.FieldCount];
                        for (var col = 0; col < sdr.FieldCount; col++)
                        {
                            values[col] = sdr[col].ToString();
                        }
                        buffTable.Add(values);
                        rowCount++;
                    }

                    string[,] pastValues = new string[rowCount, sdr.FieldCount];
                    rowCount = 0;
                    foreach (string[] rec in buffTable)
                    {
                        for (int col = 0; col < columnCount; col++)
                        {
                            pastValues[rowCount, col] = rec[col];
                        }
                        rowCount++;
                    }

                    worksheet.Range[0, 0, rowCount, columnCount].Value = pastValues;

                    // ファイルパスは実行環境に合わせて指定
                    workbook.Save(fileName);

                }
            }

            return new FileStream(fileName, FileMode.Open);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        /// <param name="userInfo">ユーザー情報</param>
        /// <returns></returns>
        public Stream ExpXlsxEPPlus(string fileName, UserInfo userInfo)
        {
            List<SqlParameter> paramList = new List<SqlParameter>();

            StringBuilder sqlstr = new StringBuilder(@$"
SELECT PostCd  
      ,Country 
      ,City    
      ,Address 
FROM MPOST 
ORDER BY PostCd
            ");

            using (SqlCommand com = new SqlCommand(sqlstr.ToString(), this.con))
            {
                if (paramList.Count > 0)
                {
                    com.Parameters.AddRange(paramList.ToArray());
                }

                using (var sdr = com.ExecuteReader())
                {
                    if (sdr.HasRows == false)
                    {
                        throw new Exception("Data Not Found");
                    }

                    List<string[]> buffTable = new List<string[]>();

                    int rowCount = 0;
                    int columnCount = sdr.FieldCount;

                    // 新規ワークブックの作成
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using ExcelPackage excelPackage = new ExcelPackage(new FileInfo(fileName));
                    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Sheet1");

                    string[] values = new string[sdr.FieldCount];
                    while (sdr.Read())
                    {
                        if (rowCount == 0)
                        {
                            // ヘッダ行作成
                            for (var col = 0; col < sdr.FieldCount; col++)
                            {
                                values[col] = sdr.GetName(col);
                            }
                            buffTable.Add(values);
                            rowCount++;
                        }

                        values = new string[sdr.FieldCount];
                        for (var col = 0; col < sdr.FieldCount; col++)
                        {
                            values[col] = sdr[col].ToString();
                        }
                        buffTable.Add(values);
                        rowCount++;
                    }

                    string[,] pastValues = new string[rowCount, sdr.FieldCount];
                    rowCount = 0;
                    foreach (string[] rec in buffTable)
                    {
                        for (int col = 0; col < columnCount; col++)
                        {
                            pastValues[rowCount, col] = rec[col];
                        }
                        rowCount++;
                    }

                    using ExcelRange range = worksheet.Cells[1, 1, rowCount, columnCount];
                    range.Value = pastValues;

                    // ファイルパスは実行環境に合わせて指定
                    excelPackage.Save();

                }
            }

            return new FileStream(fileName, FileMode.Open);
        }

        private void CreateTempTable(SqlTransaction trn, UserInfo userInfo)
        {
            // ワークテーブル作成のSQL文（同一トランザクション内で有効なテーブル）
            StringBuilder sqlstr = new StringBuilder(@$"
IF OBJECT_ID('tempdb..#MPost_Imp') IS NULL
	CREATE TABLE #MPost_Imp(
		PostCd nchar(7) NULL,
		Country nchar(10) NULL,
		City nchar(50) NULL,
		Address nchar(200) NULL
	) ON [PRIMARY]
");
            using SqlCommand com = new SqlCommand(sqlstr.ToString(), this.con, trn);
            this.AbpfExecuteNonQuery(com, userInfo);
        }

        public enum csvEncoding
        {
            utf8,
            sjis
        }

        public enum csvFieldTerminator
        {
            comma,
            tab
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        /// <param name="userInfo">ユーザー情報</param>
        /// <returns></returns>
        public int ImpCsv(
            IFormFile uploadFile, 
            SqlTransaction trn, 
            UserInfo userInfo, 
            csvEncoding encoding = csvEncoding.utf8,
            csvFieldTerminator terminator = csvFieldTerminator.comma)
        {

            /*
             事前準備 （CSV取込でエラーが発生する場合、この事前準備が整っていない可能性が高いです）

-- ① 暗号キー作成
CREATE MASTER KEY ENCRYPTION BY PASSWORD = 'zaq1"WSX';

-- ② Azure Storage に対する認証情報をSQL Server に作成
   SECRETに設定しているtokenは、Azure Storageの管理画面にて長期10年？で発行しています

CREATE DATABASE SCOPED CREDENTIAL lin_bpf_prot_hdd_temp_Credential 
WITH IDENTITY = 'SHARED ACCESS SIGNATURE',
SECRET = 'sp=r&st=2021-02-17T10:09:59Z&se=2031-02-17T18:09:59Z&spr=https&sv=2020-02-10&sr=c&sig=7Pnl6Mkne0eZ178SnDSnTUcJA1sfh4WPYComn0B2J1U%3D';

-- ③ Azure Storage に対する接続情報をSQL Server に作成
CREATE EXTERNAL DATA SOURCE lin_bpf_prot_hdd_temp_Storage
WITH (  TYPE = BLOB_STORAGE, 
        LOCATION = 'https://linabpfprotohdd.blob.core.windows.net', 
        CREDENTIAL= lin_bpf_prot_hdd_temp_Credential);


            構文例
BULK INSERT #MPost_Imp
FROM 'lin-abpf-prot-hdd-temp/ExpCsv_20210215_153622.csv'
WITH (  DATA_SOURCE = 'lin_bpf_prot_hdd_temp_Storage',
        FORMAT='CSV', CODEPAGE = 65001, --UTF-8 encoding
        FIRSTROW=2,
        TABLOCK); 

             
             */

            // ↓ 取得データの業務チェックを実施する


            // ↑ 取得データの業務チェックを実施する

            CreateTempTable(trn, userInfo); // ワークテーブル作成のSQL文

            // Blobストレージにファイルをアップロード
            const int START_ROW = 2;

            string fileName = Path.GetFileName(uploadFile.FileName).Replace(".csv", "") + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmssfff") + ".csv";
            AbpfStorageUtil.PutStorageBlob(ContainerName, fileName, uploadFile, userInfo);

            // csvオプション

            // コードページ指定 （65001 : UTF-8, 932 : Shift-Jis ）
            // 参考URL：https://www.ipentec.com/document/windows-codepage-list
            int CodePage = encoding == csvEncoding.utf8 ? 65001 : 932;

            // 区切り文字指定
            string FieldTerminator = terminator == csvFieldTerminator.comma ? "," : "\t";

            // ワークテーブル作成のSQL文（同一トランザクション内で有効なテーブル）
            StringBuilder sqlstr = new StringBuilder(@$"
BULK INSERT #MPost_Imp
FROM '{ContainerName}/{fileName}'
WITH (  DATA_SOURCE = '{DataSourceName}',
        FORMAT='CSV', 
        CODEPAGE = {CodePage},
        FIELDTERMINATOR = '{FieldTerminator}',
        FIRSTROW={START_ROW},
        TABLOCK); 
");
            using SqlCommand com = new SqlCommand(sqlstr.ToString(), this.con, trn);

            int ret = 0;
            try
            {
                ret = this.AbpfExecuteNonQuery(com, userInfo);
            }
            catch (Exception)
            {
                throw;
            }


            // ↓ ワークテーブルに取り込んだデータを処理するストプロやプログラムを実行

            // ↑ ワークテーブルに取り込んだデータを処理するストプロやプログラムを実行

            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        /// <param name="userInfo">ユーザー情報</param>
        /// <returns></returns>
        public int ImpXlsxDioDocs(IFormFile uploadFile, SqlTransaction trn, UserInfo userInfo)
        {
            // 取込の準備（項目の列番号と開始行数を定義）
            // DioDocsの場合、0相対
            const int COL_PostCd = 0;
            const int COL_Country = 1;
            const int COL_City = 2;
            const int COL_Address = 3;
            const int START_ROW = 1;

            // アップロードファイルを開く
            var workbook = new GrapeCity.Documents.Excel.Workbook();
            workbook.Open(uploadFile.OpenReadStream(), OpenFileFormat.Xlsx);
            IWorksheet worksheet = workbook.Worksheets[0];

            // 使用中セル範囲の取得
            object[,] values = (object[,])worksheet.UsedRange.Value;

            var range = worksheet.UsedRange;

            // ↓ 取得データの業務チェックを実施する

            // 範囲情報の取得
            Console.WriteLine("usedrange.RowCount" + range.RowCount);
            Console.WriteLine("usedrange.ColumnCount" + range.ColumnCount);
            Console.WriteLine("usedrange.LastRow" + range.LastRow);
            Console.WriteLine("usedrange.LastColumn" + range.LastColumn);

            // ↑ 取得データの業務チェックを実施する

            CreateTempTable(trn, userInfo); // ワークテーブル作成のSQL文

            // SQLの生成と実行（速度維持の為に、複数行単位でInsert実行）
            StringBuilder sqlstr = new StringBuilder();
            int intCnt = 0; // 取込した件数
            int intRet = 0; // AbpfExecuteNonQueryの戻り値を受け取る変数
            List<SqlParameter> paramList = new List<SqlParameter>();

            for (int i = START_ROW; i <= range.LastRow; i++) // 開始行(ヘッダを除く)から、最終行までをループ
            {

                if (sqlstr.Length == 0)
                {
                    // sqlstrが空の場合
                    // INSERT INTO #MPost_Imp(PostCd, Country, City, Address) VALUES
                    sqlstr.AppendLine($@"INSERT INTO #MPost_Imp(PostCd, Country, City, Address) VALUES");
                }
                else
                {
                    // sqlstrが空でない場合
                    // [, ]を付ける
                    sqlstr.AppendLine($@",");
                }

                // 取込対象行をカウントする
                intCnt++;
                sqlstr.AppendLine($@"(@PostCd{i}, @Country{i}, @City{i}, @Address{i})");
                paramList.Add(AbpfDBUtil.GetSqlParameter("PostCd" + i, values[i, COL_PostCd].ToString()));
                paramList.Add(AbpfDBUtil.GetSqlParameter("Country" + i, values[i, COL_Country].ToString()));
                paramList.Add(AbpfDBUtil.GetSqlParameter("City" + i, values[i, COL_City].ToString()));
                paramList.Add(AbpfDBUtil.GetSqlParameter("Address" + i, values[i, COL_Address].ToString()));

                // 実行の条件（行数で判断） 
                // パラメータ数の上限が2000なので、上限を超えないようにまとめて実行する
                // 1レコード、4項目の場合 ＝ 最大500行を一括実行可能
                if (intCnt % 500 == 0)
                {
                    using SqlCommand com = new SqlCommand(sqlstr.ToString(), this.con, trn);
                    com.Parameters.AddRange(paramList.ToArray());
                    // SQL実行
                    intRet = this.AbpfExecuteNonQuery(com, userInfo);
                    if (intRet == 0)
                    {
                        // 件数 = 0 ： 異常終了 → -1 でリターン
                        return -1;
                    }
                    else
                    {
                        // 件数 > 0 ： 正常終了 → 次に備えてsqlとparameterをクリア
                        sqlstr.Clear();
                        paramList.Clear();
                    }
                }
            }

            // 実行の条件（sqlstrのサイズで判断）
            if (sqlstr.Length > 0) // 未実行のSQL残りが存在する場合に実行
            {
                using SqlCommand com = new SqlCommand(sqlstr.ToString(), this.con, trn);
                com.Parameters.AddRange(paramList.ToArray());
                // SQL実行
                intRet = this.AbpfExecuteNonQuery(com, userInfo);
                if (intRet == 0)
                {
                    // 件数 = 0 ： 異常終了 → -1 でリターン
                    return -1;
                }
            }

            // Excelファイルの内容をワークテーブルに読み込み完了

            // ↓ ワークテーブルに取り込んだデータを処理するストプロやプログラムを実行

            // ↑ ワークテーブルに取り込んだデータを処理するストプロやプログラムを実行

            return intCnt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        /// <param name="userInfo">ユーザー情報</param>
        /// <returns></returns>
        public int ImpXlsxEPPlus(IFormFile uploadFile, SqlTransaction trn, UserInfo userInfo)
        {
            // 取込の準備（項目の列番号と開始行数を定義）
            // EPPlusの場合、1相対
            const int COL_PostCd = 1;
            const int COL_Country = 2;
            const int COL_City = 3;
            const int COL_Address = 4;
            const int START_ROW = 2;

            // アップロードファイルを開く
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using ExcelPackage excelPackage = new ExcelPackage(uploadFile.OpenReadStream());
            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets["Sheet1"];


            // EPPlusの場合、使用中セル範囲の取得ができない

            // ↓ 取得データの業務チェックを実施する


            // ↑ 取得データの業務チェックを実施する

            CreateTempTable(trn, userInfo); // ワークテーブル作成のSQL文

            // SQLの生成と実行（速度維持の為に、複数行単位でInsert実行）
            StringBuilder sqlstr = new StringBuilder();
            int intCnt = 0; // 取込した件数
            int intRet = 0; // AbpfExecuteNonQueryの戻り値を受け取る変数
            List<SqlParameter> paramList = new List<SqlParameter>();

            int i = START_ROW;
            
            while(worksheet.Cells[i, COL_PostCd].Value != null) // 開始行(ヘッダを除く)から、最終行までをループ
            {

                if (sqlstr.Length == 0)
                {
                    // sqlstrが空の場合
                    // INSERT INTO #MPost_Imp(PostCd, Country, City, Address) VALUES
                    sqlstr.AppendLine($@"INSERT INTO #MPost_Imp(PostCd, Country, City, Address) VALUES");
                }
                else
                {
                    // sqlstrが空でない場合
                    // [, ]を付ける
                    sqlstr.AppendLine($@",");
                }

                // 取込対象行をカウントする
                intCnt++;
                sqlstr.AppendLine($@"(@PostCd{i}, @Country{i}, @City{i}, @Address{i})");
                paramList.Add(AbpfDBUtil.GetSqlParameter("PostCd" + i, worksheet.Cells[i, COL_PostCd].Value.ToString()));
                paramList.Add(AbpfDBUtil.GetSqlParameter("Country" + i, worksheet.Cells[i, COL_Country].Value == null ? string.Empty : worksheet.Cells[i, COL_Country].Value.ToString()));
                paramList.Add(AbpfDBUtil.GetSqlParameter("City" + i, worksheet.Cells[i, COL_City].Value == null ? string.Empty : worksheet.Cells[i, COL_City].Value.ToString()));
                paramList.Add(AbpfDBUtil.GetSqlParameter("Address" + i, worksheet.Cells[i, COL_Address].Value == null ? string.Empty : worksheet.Cells[i, COL_Address].Value.ToString()));

                // 実行の条件（行数で判断） 
                // パラメータ数の上限が2000なので、上限を超えないようにまとめて実行する
                // 1レコード、4項目の場合 ＝ 最大500行を一括実行可能
                if (intCnt % 500 == 0)
                {
                    using SqlCommand com = new SqlCommand(sqlstr.ToString(), this.con, trn);
                    com.Parameters.AddRange(paramList.ToArray());
                    // SQL実行
                    intRet = this.AbpfExecuteNonQuery(com, userInfo);
                    if (intRet == 0)
                    {
                        // 件数 = 0 ： 異常終了 → -1 でリターン
                        return -1;
                    }
                    else
                    {
                        // 件数 > 0 ： 正常終了 → 次に備えてsqlとparameterをクリア
                        sqlstr.Clear();
                        paramList.Clear();
                    }
                }

                i++;
            }

            // 実行の条件（sqlstrのサイズで判断）
            if (sqlstr.Length > 0) // 未実行のSQL残りが存在する場合に実行
            {
                using SqlCommand com = new SqlCommand(sqlstr.ToString(), this.con, trn);
                com.Parameters.AddRange(paramList.ToArray());
                // SQL実行
                intRet = this.AbpfExecuteNonQuery(com, userInfo);
                if (intRet == 0)
                {
                    // 件数 = 0 ： 異常終了 → -1 でリターン
                    return -1;
                }
            }

            // Excelファイルの内容をワークテーブルに読み込み完了

            // ↓ ワークテーブルに取り込んだデータを処理するストプロやプログラムを実行

            // ↑ ワークテーブルに取り込んだデータを処理するストプロやプログラムを実行

            return intCnt;
        }
    }
}
