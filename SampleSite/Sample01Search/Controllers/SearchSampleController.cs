// <copyright file="SearchSampleController.cs" company="Above Beyond Project Framework">
// Copyright (c) Above Beyond Project Framework. All rights reserved.
// </copyright>

namespace Search01Search.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Sample01Base.Controllers;

    /// <summary>
    /// 検索サンプルコントローラー
    /// </summary>
    public class SearchSampleController : SampleController
    {
        private ILoggerFactory logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchSampleController"/> class.
        /// </summary>
        /// <param name="logger">標準ロガー</param>
        public SearchSampleController(
            ILoggerFactory logger)
            : base(logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Index: 初期表示
        /// </summary>
        /// <returns>View</returns>
        public ActionResult Index()
        {
            return this.View();
        }
    }
}
