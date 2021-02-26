// <copyright file="MailSampleController.cs" company="Above Beyond Project Framework">
// Copyright (c) Above Beyond Project Framework. All rights reserved.
// </copyright>

namespace Sample06Mail.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Sample01Base.Controllers;

    /// <summary>
    /// メールサンプル画面
    /// </summary>
    public class MailSampleController : SampleController
    {
        private ILoggerFactory logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="MailSampleController"/> class.
        /// </summary>
        /// <param name="logger">標準ロガー</param>
        public MailSampleController(ILoggerFactory logger)
            : base(logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Index
        /// </summary>
        /// <returns>View</returns>
        public ActionResult Index()
        {
            return this.View();
        }
    }
}
