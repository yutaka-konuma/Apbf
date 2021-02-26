// <copyright file="StorageSampleController.cs" company="Above Beyond Project Framework">
// Copyright (c) Above Beyond Project Framework. All rights reserved.
// </copyright>

namespace Sample04Storage.Controllers
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Sample01Base.Controllers;
    using Sample04Storage.Service;

    /// <summary>
    /// ファイル入出力画面コントローラー.
    /// </summary>
    public class StorageSampleAPIController : SampleController
    {
        private ILoggerFactory logger;

        private StorageSampleService storageSampleService;

        /// <summary>
        /// Initializes a new instance of the <see cref="StorageSampleAPIController"/> class.
        /// </summary>
        /// <param name="logger">標準ロガー.</param>
        public StorageSampleAPIController(
            ILoggerFactory logger)
            : base(logger)
        {
            this.logger = logger;
            this.storageSampleService = new StorageSampleService(logger);
        }

        /// <summary>
        /// GET: StorageSample/GetStorageBlob/
        /// </summary>
        /// <returns>全ファイルの名称リスト</returns>
        [Route("StorageSample/GetStorageBlob/")]
        [HttpGet]
        public ActionResult GetStorageBlob()
        {
            // 指定コンテナの全ファイルの名称リスト取得
            var jsonRet = this.Json(storageSampleService.GetStorageBlob(this.GetUserInfo(this.HttpContext)));
            return jsonRet;
        }

        /// <summary>
        /// Post: StorageSample/PutStorageBlob/
        /// </summary>
        /// <param name="uploadFile">アップロードファイル</param>
        /// <returns>0:アップロード完了 -1:失敗 -9:コンテナ名称がない</returns>
        [Route("StorageSample/PutStorageBlob/")]
        [HttpPost]
        public ActionResult PutStorageBlob(IFormFile uploadFile)
        {
            // 指定コンテナにファイルを保存
            return this.Json(storageSampleService.PutStorageBlob(uploadFile, this.GetUserInfo(this.HttpContext)));
        }

        /// <summary>
        /// get: StorageSample/DownloadStorageBlob/
        /// </summary>
        /// <param name="fileName">ダウンロードファイル名</param>
        /// <returns>ダウンロードファイルレスポンス</returns>
        [Route("StorageSample/DownloadStorageBlob/{fileName}")]
        [HttpGet]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult DownloadStorageBlob(string fileName)
        {
            // 指定コンテナのファイルを取得（ストリーム形式）
            return this.File(storageSampleService.DownloadStorageBlob(fileName, this.GetUserInfo(this.HttpContext)), "text/plain", fileName);
        }
    }
}
