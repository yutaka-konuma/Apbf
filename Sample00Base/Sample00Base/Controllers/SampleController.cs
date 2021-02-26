// <copyright file="SampleController.cs" company="Above Beyond Project Framework">
// Copyright (c) Above Beyond Project Framework. All rights reserved.
// </copyright>

namespace Sample01Base.Controllers
{
    using AbpfBase.Controllers;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// サンプルアプリケーション共通コントローラー
    /// </summary>
    public class SampleController : AbpfController
    {
        private ILoggerFactory logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="SampleController"/> class.
        /// </summary>
        /// <param name="logger">標準ロガー</param>
        public SampleController(
            ILoggerFactory logger)
            : base(logger)
        {
            this.logger = logger;
        }
    }
}
