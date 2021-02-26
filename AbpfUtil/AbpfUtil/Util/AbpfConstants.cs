// <copyright file="AbpfConstants.cs" company="Above Beyond Project Framework">
// Copyright (c) Above Beyond Project Framework. All rights reserved.
// </copyright>

namespace AbpfUtil.Util
{
    using System;

    /// <summary>
    /// 共通基盤定数
    /// </summary>
    public static class AbpfConstants
    {
        /// <summary>
        /// 送信結果
        /// </summary>
        public enum MailResult
        {
            /// <summary>
            /// 送信成功
            /// </summary>
            Success = 1,

            /// <summary>
            /// 送信エラー
            /// </summary>
            Error = 0,
        }

        /// <summary>
        /// アップロード結果
        /// </summary>
        public enum AzureUploadResult
        {
            /// <summary>
            /// アップロード成功
            /// </summary>
            Success = 0,

            /// <summary>
            /// アップロード失敗
            /// </summary>
            Error = -1,

            /// <summary>
            /// コンテナが見つからない
            /// </summary>
            ContainerNotFound = -9,
        }
    }
}
