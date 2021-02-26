// <copyright file="StorageSampleController.cs" company="Above Beyond Project Framework">
// Copyright (c) Above Beyond Project Framework. All rights reserved.
// </copyright>

namespace Sample04Storage.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Sample01Base.Controllers;
    using Sample04Storage.Service;

    /// <summary>
    /// ファイル入出力画面コントローラー.
    /// </summary>
    public class StorageSampleController : SampleController
    {
        private ILoggerFactory logger;

        private StorageSampleService storageSampleService;

        /// <summary>
        /// Initializes a new instance of the <see cref="StorageSampleAPIController"/> class.
        /// </summary>
        /// <param name="logger">標準ロガー.</param>
        public StorageSampleController(
            ILoggerFactory logger)
            : base(logger)
        {
            this.logger = logger;
            this.storageSampleService = new StorageSampleService(logger);
        }

        /// <summary>
        /// Index：初期表示.
        /// </summary>
        /// <returns>View.</returns>
        public ActionResult Index()
        {
            return this.View();
        }
    }
}
