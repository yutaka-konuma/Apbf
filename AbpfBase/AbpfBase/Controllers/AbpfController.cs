// <copyright file="AbpfController.cs" company="Above Beyond Project Framework">
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
    /// Abpf基盤コントローラー
    /// </summary>
    public class AbpfController : Controller
    {
        private ILoggerFactory logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AbpfController"/> class.
        /// コンストラクタ
        /// </summary>
        /// <remarks>
        /// ログ出力する
        /// </remarks>
        /// <param name="logger">標準ロガー</param>
        public AbpfController(
            ILoggerFactory logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// デストラクタ
        /// </summary>
        /// <remarks>
        /// ログ出力する
        /// </remarks>
        /// <param name="disposing">disposing</param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
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
