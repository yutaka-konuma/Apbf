// <copyright file="GoodsMasterController.cs" company="Above Beyond Project Framework">
// Copyright (c) Above Beyond Project Framework. All rights reserved.
// </copyright>

namespace Sample02GoodsMaster.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Sample01Base.Controllers;

    /// <summary>
    /// 商品マスタメンテナンス画面コントローラー
    /// </summary>
    public class GoodsMasterController : SampleController
    {
        private ILoggerFactory logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="GoodsMasterController"/> class.
        /// </summary>
        /// <param name="logger">標準ロガー</param>
        public GoodsMasterController(
            ILoggerFactory logger)
            : base(logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Index：初期表示
        /// </summary>
        /// <returns>View</returns>
        public ActionResult Index()
        {
            return this.View();
        }
    }
}
