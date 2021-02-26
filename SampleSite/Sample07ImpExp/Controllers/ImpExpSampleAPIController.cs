// <copyright file="SearchSampleAPIController.cs" company="Above Beyond Project Framework">
// Copyright (c) Above Beyond Project Framework. All rights reserved.
// </copyright>

namespace Search01Search.Controllers
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Sample01Base.Controllers;
    using Sample07ImpExp.Service;
    using System;
    using System.Diagnostics;

    /// <summary>
    /// CSV/Excelファイル作成/取込サンプルAPIコントローラー
    /// </summary>
    public class ImpExpSampleAPIController : SampleController
    {
        private ILoggerFactory logger;

        private ImpExpSampleService impExpSampleService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchSampleAPIController"/> class.
        /// </summary>
        /// <param name="logger">標準ロガー</param>
        public ImpExpSampleAPIController(
            ILoggerFactory logger)
            : base(logger)
        {
            this.logger = logger;
            this.impExpSampleService = new ImpExpSampleService(this.logger);
        }

        /// <summary>
        /// get: ImpExpSample/ExpCsv/
        /// </summary>
        /// <param name="fileName">ダウンロードファイル名</param>
        /// <returns>ダウンロードファイルレスポンス</returns>
        [Route("ImpExpSample/ExpCsv/")]
        [HttpGet]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult ExpCsv()
        {
            string fileName = "ExpCsv_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".csv";

            // 指定コンテナのファイルを取得（ストリーム形式）
            return this.File(impExpSampleService.ExpCsv(fileName, this.GetUserInfo(this.HttpContext)), "text/plain", fileName);
        }

        /// <summary>
        /// get: ImpExpSample/ExpXlsx/
        /// </summary>
        /// <param name="fileName">ダウンロードファイル名</param>
        /// <returns>ダウンロードファイルレスポンス</returns>
        [Route("ImpExpSample/ExpXlsxDioDocs/")]
        [HttpGet]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult ExpXlsxDioDocs()
        {

            string fileName = "ExpXlsxDioDocs_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xlsx";
            string fullFilePath = "C:\\log\\" + fileName;

            return this.File(impExpSampleService.ExpXlsxDioDocs(fullFilePath, this.GetUserInfo(this.HttpContext)), "text/plain", fileName);
        }

        /// <summary>
        /// get: ImpExpSample/ExpXlsx/
        /// </summary>
        /// <param name="fileName">ダウンロードファイル名</param>
        /// <returns>ダウンロードファイルレスポンス</returns>
        [Route("ImpExpSample/ExpXlsxEPPlus/")]
        [HttpGet]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult ExpXlsxEPPlus()
        {

            string fileName = "ExpXlsxEPPlus_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xlsx";
            string fullFilePath = "C:\\log\\" + fileName;

            return this.File(impExpSampleService.ExpXlsxEPPlus(fullFilePath, this.GetUserInfo(this.HttpContext)), "text/plain", fileName);
        }

        /// <summary>
        /// Post: ImpExpSample/ImpCsv/
        /// </summary>
        /// <param name="uploadFile">アップロードファイル</param>
        /// <returns>0:アップロード完了 -1:失敗 -9:コンテナ名称がない</returns>
        [Route("ImpExpSample/ImpCsv/")]
        [HttpPost]
        public ActionResult ImpCsv(IFormFile uploadFile)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            int ret = impExpSampleService.ImpCsv(uploadFile, this.GetUserInfo(this.HttpContext));

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;

            return this.Json("取込件数(Csv)：" + ret + "件(" + String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10) + ")");
        }

        /// <summary>
        /// Post: ImpExpSample/ImpXlsxDioDocs/
        /// </summary>
        /// <param name="uploadFile">アップロードファイル</param>
        /// <returns>0:アップロード完了 -1:失敗 -9:コンテナ名称がない</returns>
        [Route("ImpExpSample/ImpXlsxDioDocs/")]
        [HttpPost]
        public ActionResult ImpXlsxDioDocs(IFormFile uploadFile)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            int ret = impExpSampleService.ImpXlsxDioDocs(uploadFile, this.GetUserInfo(this.HttpContext));

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;

            return this.Json("取込件数(DioDocs)：" + ret + "件(" + String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10) + ")");
        }

        /// <summary>
        /// Post: ImpExpSample/ImpXlsxEPPlus/
        /// </summary>
        /// <param name="uploadFile">アップロードファイル</param>
        /// <returns>0:アップロード完了 -1:失敗 -9:コンテナ名称がない</returns>
        [Route("ImpExpSample/ImpXlsxEPPlus/")]
        [HttpPost]
        public ActionResult ImpXlsxEPPlus(IFormFile uploadFile)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            int ret = impExpSampleService.ImpXlsxEPPlus(uploadFile, this.GetUserInfo(this.HttpContext));

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;

            return this.Json("取込件数(EPPlus)：" + ret + "件(" + String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10) + ")");
        }
    }
}






//// Get the connection string from app settings
//string connectionString = AbpfConfigUtil.GetConnectionString("Storage");

//// Instantiate a ShareClient which will be used to create and manipulate the file share
//ShareClient share = new ShareClient(connectionString, "linabpfprotohddfile01");

//// Create the share if it doesn't already exist
//share.CreateIfNotExists();

//// Ensure that the share exists
//if (share.Exists())
//{
//    Console.WriteLine($"Share created: {share.Name}");

//    // Get a reference to the sample directory
//    ShareDirectoryClient directory = share.GetDirectoryClient("ExcelCreaterTemp");

//    directory.CreateIfNotExists();

//    // Ensure that the directory exists
//    if (directory.Exists())
//    {
//        // Get a reference to a file object
//        ShareFileClient file = directory.GetFileClient(fileName);

//        // Ensure that the file exists
//        if (file.Exists())
//        {
//            Console.WriteLine($"File exists: {file.Name}");

//            // Download the file
//            ShareFileDownloadInfo download = file.Download();

//            // Save the data to a local file, overwrite if the file already exists
//            //using (FileStream stream = File.OpenWrite(@"downloadedLog1.txt"))
//            //{
//            //    download.Content.CopyTo(stream);
//            //    stream.Flush();
//            //    stream.Close();

//            //    // Display where the file was saved
//            //    Console.WriteLine($"File downloaded: {stream.Name}");
//            //}
//        }
//    }
//}
//else
//{
//    Console.WriteLine($"CreateShareAsync failed");
//}




// ExcelCreator インスタンス生成 (以下、.NET Framework 系で実行した場合、動作可能）
/*
            Creator creator1 = new Creator();

            //【1】Excel ファイル新規作成
            creator1.CreateBook(fullFilePath, 1, xlsxVersion.ver2013);

            //【2】値の設定
            creator1.DefaultFontName = "メイリオ";  // デフォルトフォント
            creator1.DefaultFontPoint = 10;         // デフォルトフォントポイント
            creator1.SheetName = "商品売上明細書";  // シート名
                                             // 行の高さ、列幅の調整
            creator1.Cell("1").RowHeight = 30;
            creator1.Cell("2").RowHeight = 15;
            creator1.Cell("3").RowHeight = 18;
            creator1.Cell("4:12").RowHeight = 15;
            creator1.Cell("A").ColWidth = 1.88;
            creator1.Cell("B:R").ColWidth = 3.13;
            creator1.Cell("S").ColWidth = 1.88;
            // 値、書式設定
            creator1.Cell("B1").Value = "商品売上明細書";
            creator1.Cell("B1:R1").Attr.MergeCells = true;
            creator1.Cell("B1").Attr.FontPoint = 18;
            creator1.Cell("B1").Attr.FontStyle = AdvanceSoftware.ExcelCreator.FontStyle.Bold;
            creator1.Cell("B1").Attr.HorizontalAlignment = HorizontalAlignment.Center;
            creator1.Cell("B1").Attr.FontColor2 = xlColor.White;
            creator1.Cell("B1").Attr.BackColor = Color.FromArgb(91, 155, 213);
            creator1.Cell("B3").Value = "商品名";
            creator1.Cell("B3:G3").Attr.MergeCells = true;
            creator1.Cell("H3").Value = "数量";
            creator1.Cell("H3:J3").Attr.MergeCells = true;
            creator1.Cell("K3").Value = "単価";
            creator1.Cell("K3:N3").Attr.MergeCells = true;
            creator1.Cell("O3").Value = "金額";
            creator1.Cell("O3:R3").Attr.MergeCells = true;
            creator1.Cell("B3:R3").Attr.HorizontalAlignment = HorizontalAlignment.Center;
            creator1.Cell("B3:R3").Attr.FontColor2 = xlColor.White;
            creator1.Cell("B4:G4").Attr.MergeCells = true;
            creator1.Cell("H4:J4").Attr.MergeCells = true;
            creator1.Cell("K4:N4").Attr.MergeCells = true;
            creator1.Cell("O4:R4").Attr.MergeCells = true;
            for (int i = 0; i < 5; i++)
            {
                creator1.RowCopy(3, 4 + i);
            }
            creator1.Cell("B3:R3").Attr.BackColor = Color.FromArgb(91, 155, 213);
            creator1.Cell("B3:R9").Attr.Box(BoxType.Ltc, BorderStyle.Thin, Color.FromArgb(91, 155, 213));
            creator1.Cell("B3:R9").Attr.Box(BoxType.Box, BorderStyle.Medium, Color.FromArgb(91, 155, 213));
            creator1.Cell("B4").Value = "ExcelCreator 2016";
            creator1.Cell("H4").Value = 10;
            creator1.Cell("K4").Value = 64000;
            creator1.Cell("O4").Func("=H4*K4", null);
            creator1.Cell("B5").Value = "VB-Report 8";
            creator1.Cell("H5").Value = 8;
            creator1.Cell("K5").Value = 85000;
            creator1.Cell("O5").Func("=H5*K5", null);
            creator1.Cell("B6").Value = "ExcelWebForm";
            creator1.Cell("H6").Value = 5;
            creator1.Cell("K6").Value = 70000;
            creator1.Cell("O6").Func("=H6*K6", null);
            creator1.Cell("K4:N9").Attr.Format = "#,##0_ ";
            creator1.Cell("O4:R9").Attr.Format = @"¥#,##0;[赤]\#,##0";
            creator1.Cell("B4:R4").Attr.LineBottom(BorderStyle.Dotted, Color.FromArgb(91, 155, 213));
            creator1.Cell("B5:R5").Attr.LineTop(BorderStyle.Dotted, Color.FromArgb(91, 155, 213));
            creator1.Cell("B5:R5").Attr.LineBottom(BorderStyle.Dotted, Color.FromArgb(91, 155, 213));
            creator1.Cell("B6:R6").Attr.LineTop(BorderStyle.Dotted, Color.FromArgb(91, 155, 213));
            creator1.Cell("B6:R6").Attr.LineBottom(BorderStyle.Dotted, Color.FromArgb(91, 155, 213));
            creator1.Cell("B7:R7").Attr.LineTop(BorderStyle.Dotted, Color.FromArgb(91, 155, 213));
            creator1.Cell("B7:R7").Attr.LineBottom(BorderStyle.Dotted, Color.FromArgb(91, 155, 213));
            creator1.Cell("B8:R8").Attr.LineTop(BorderStyle.Dotted, Color.FromArgb(91, 155, 213));
            creator1.Cell("B8:R8").Attr.LineBottom(BorderStyle.Dotted, Color.FromArgb(91, 155, 213));
            creator1.Cell("B9:R9").Attr.LineTop(BorderStyle.Dotted, Color.FromArgb(91, 155, 213));
            creator1.Cell("B5:R5").Attr.BackColor = Color.FromArgb(221, 235, 247);
            creator1.Cell("B7:R7").Attr.BackColor = Color.FromArgb(221, 235, 247);
            creator1.Cell("B9:R9").Attr.BackColor = Color.FromArgb(221, 235, 247);
            creator1.Cell("K10:N10").Attr.MergeCells = true;
            creator1.Cell("K10:N10").Attr.HorizontalAlignment = HorizontalAlignment.Center;
            creator1.Cell("K10:N10").Attr.FontColor2 = xlColor.White;
            creator1.Cell("K10:N10").Attr.BackColor = Color.FromArgb(91, 155, 213);
            creator1.Cell("O10:R10").Attr.MergeCells = true;
            creator1.Cell("O10:R10").Attr.Format = @"¥#,##0;[赤]\#,##0";
            creator1.Cell("K10:R10").Attr.Box(BoxType.Ltc, BorderStyle.Thin, Color.FromArgb(91, 155, 213));
            creator1.Cell("K10:R10").Attr.Box(BoxType.Box, BorderStyle.Medium, Color.FromArgb(91, 155, 213));
            for (int i = 0; i < 2; i++)
            {
                creator1.RowCopy(9, 10 + i);
            }
            creator1.Cell("K10").Value = "小計";
            creator1.Cell("O10").Func("=SUM(O4:R8)", null);
            creator1.Cell("K11").Value = "消費税";
            creator1.Cell("O11").Func("=O10*0.08", null);
            creator1.Cell("K12").Value = "合計";
            creator1.Cell("O12").Func("=O10+O11", null);

            //【3】Excel ファイルクローズ
            creator1.CloseBook(true);
*/