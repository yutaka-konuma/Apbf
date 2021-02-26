// <copyright file="BasisController.cs" company="Above Beyond Project Framework">
// Copyright (c) Above Beyond Project Framework. All rights reserved.
// </copyright>

namespace Sample03Basis.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Sample01Base.Controllers;

    /// <summary>
    /// 基礎テーブル画面コントローラー
    /// </summary>
    public class BasisController : SampleController
    {
        private ILoggerFactory logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="BasisController"/> class.
        /// </summary>
        /// <param name="logger">標準ロガー</param>
        public BasisController(
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
