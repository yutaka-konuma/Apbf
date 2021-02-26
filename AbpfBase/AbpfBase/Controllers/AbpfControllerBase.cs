// <copyright file="AbpfControllerBase.cs" company="Above Beyond Project Framework">
// Copyright (c) Above Beyond Project Framework. All rights reserved.
// </copyright>

namespace AbpfBase.Controllers
{
    using AbpfUtil.Models;
    using AbpfUtil.Util;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// AbpfAPI基盤コントローラー
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AbpfControllerBase : ControllerBase
    {
        private ILoggerFactory logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AbpfControllerBase"/> class.
        /// コンストラクタ
        /// </summary>
        /// <remarks>
        /// ログ出力する
        /// </remarks>
        /// <param name="logger">標準ロガー</param>
        public AbpfControllerBase(
            ILoggerFactory logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// ユーザー情報取得
        /// </summary>
        /// <param name="httpContext">Context</param>
        /// <returns>ユーザー情報</returns>
        protected UserInfo GetUserInfo(HttpContext httpContext)
        {
            return AbpfCommonUtil.GetUserInfo(httpContext);
        }
    }
}
