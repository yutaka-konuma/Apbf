// <copyright file="SampleAPIControllerBase.cs" company="Above Beyond Project Framework">
// Copyright (c) Above Beyond Project Framework. All rights reserved.
// </copyright>

namespace Sample01Base.Controllers
{
    using AbpfBase.Controllers;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// サンプルアプリケーション共通APIコントローラー
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class SampleAPIControllerBase : AbpfControllerBase
    {
        private ILoggerFactory logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="SampleAPIControllerBase"/> class.
        /// </summary>
        /// <param name="logger">標準ロガー</param>
        public SampleAPIControllerBase(
            ILoggerFactory logger)
            : base(logger)
        {
            this.logger = logger;
        }
    }
}
