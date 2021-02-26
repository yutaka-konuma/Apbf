// <copyright file="SampleServiceBase.cs" company="Above Beyond Project Framework">
// Copyright (c) Above Beyond Project Framework. All rights reserved.
// </copyright>

namespace Sample01Base.Service
{
    using AbpfBase.Service;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// サンプルアプリケーション共通サービス
    /// </summary>
    public class SampleServiceBase : AbpfServiceBase
    {
        // 標準ロガー
        private readonly ILoggerFactory logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="SampleServiceBase"/> class.
        /// </summary>
        /// <param name="logger">標準ロガー</param>
        public SampleServiceBase(ILoggerFactory logger, bool readOnlyFlg = false)
            : base(logger, readOnlyFlg)
        {
            this.logger = logger;
        }
    }
}
