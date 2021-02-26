// <copyright file="AbpfConfigUtil.cs" company="Above Beyond Project Framework">
// Copyright (c) Above Beyond Project Framework. All rights reserved.
// </copyright>

namespace AbpfUtil.Util
{
    using System;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// 構成ファイル処理
    /// </summary>
    public static class AbpfConfigUtil
    {
        /// <summary>
        /// ConnectionStringセクション
        /// </summary>
        public const string SectionConnectingString = "ConnectingString";

        /// <summary>
        /// Default
        /// </summary>
        public const string Default = "Default";

        /// <summary>
        /// Storage
        /// </summary>
        public const string Storage = "Storage";

        /// <summary>
        /// Mailセクション
        /// </summary>
        public const string SectionMail = "Mail";

        /// <summary>
        /// AESキー(半角英数16桁）
        /// </summary>
        public const string Key = "Key";

        /// <summary>
        /// クライアントID
        /// </summary>
        public const string ClientId = "clientId";

        /// <summary>
        /// クライアントシークレット
        /// </summary>
        public const string ClientSecret = "clientSecret";

        /// <summary>
        /// テナントID
        /// </summary>
        public const string TenantId = "tenantId";

        /// <summary>
        /// EwsScopes
        /// </summary>
        public const string EwsScopes = "ewsScopes";

        /// <summary>
        /// EwsUrl
        /// </summary>
        public const string EwsUrl = "ewsUrl";

        /// <summary>
        /// メールアカウント
        /// </summary>
        public const string AccountId = "accountId";

        /// <summary>
        /// パスワード（暗号化）
        /// </summary>
        public const string Password = "password";

        /// <summary>
        /// 送信元（固定）
        /// </summary>
        public const string FromAddress = "fromAddress";

        /// <summary>
        /// メールサーバ
        /// </summary>
        public const string Hostname = "hostname";

        /// <summary>
        /// ポート
        /// </summary>
        public const string Port = "port";

        /// <summary>
        /// 構成ファイル値取得
        /// </summary>
        /// <param name="section">セクション（一段目キー）</param>
        /// <param name="key">キー（二段目キー）</param>
        /// <returns>値</returns>
        public static string GetValue(string section, string key)
        {
            var config = LoadAppSettings(null);
            var result = config[section + ":" + key];
            return result;
        }

        /// <summary>
        /// 構成ファイル値取得
        /// </summary>
        /// <param name="key">キー</param>
        /// <returns>値</returns>
        public static string GetConnectionString(string key)
        {
            var config = LoadAppSettings(null);
            var result = config[SectionConnectingString + ":" + key];
            return result;
        }

        /// <summary>
        /// ConfigurationBuilder作成
        /// </summary>
        /// <param name="consoleEnv">環境変数でなく指定する場合</param>
        /// <returns>ConfigurationBuilder</returns>
        public static IConfigurationRoot LoadAppSettings(string consoleEnv)
        {
            var env = Environment.GetEnvironmentVariables();
            var environmentName = consoleEnv ?? Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            // logger.LogInformation("ASPNETCORE_ENVIRONMENT is : " + environmentName);
            // var hoge = env.GetEnumerator();
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environmentName}.json", optional: true)
                .AddEnvironmentVariables();
            return builder.Build();
        }
    }
}
