// <copyright file="AbpfServiceBase.cs" company="Above Beyond Project Framework">
// Copyright (c) Above Beyond Project Framework. All rights reserved.
// </copyright>

namespace AbpfBase.Service
{
    using System;
    using AbpfUtil.Util;
    using Microsoft.Data.SqlClient;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Abpf基盤サービス
    /// </summary>
    public class AbpfServiceBase : IDisposable
    {
        /// <summary>
        /// SQLConnection
        /// </summary>
        protected readonly SqlConnection sqlConnection;

        // 標準ロガー
        private readonly ILoggerFactory logger;

        private bool disposedValue;

        public bool IsReadOnly { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbpfServiceBase"/> class.
        /// </summary>
        /// <param name="logger">標準ロガー</param>
        public AbpfServiceBase(ILoggerFactory logger, bool readOnlyFlg = false)
        {
            try
            {
                this.logger = logger;

                this.IsReadOnly = readOnlyFlg;

                // DB再接続
                if (this.sqlConnection == null || this.sqlConnection.State != System.Data.ConnectionState.Open)
                {
                    // Config取得
                    var connectingString = readOnlyFlg == true ? AbpfConfigUtil.GetConnectionString("ReadOnly") : AbpfConfigUtil.GetConnectionString("Default");
                    this.sqlConnection = new SqlConnection(connectingString);
                    Console.WriteLine(connectingString);
                    this.sqlConnection.Open();
                }
            }
            catch (Exception e)
            {
                // HttpContext=nullでログ出力
                Console.WriteLine(e.ToString());
                AbpfLogUtil.LogError(e, null);
                throw;
            }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            // このコードを変更しないでください。クリーンアップ コードを 'Dispose(bool disposing)' メソッドに記述します
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing">disposing</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    // マネージド状態を破棄します (マネージド オブジェクト)
                    try
                    {
                        if (this.sqlConnection != null && this.sqlConnection.State == System.Data.ConnectionState.Open)
                        {
                            this.sqlConnection.Close();
                        }
                    }
                    catch (Exception e)
                    {
                        // HttpContext=nullでログ出力
                        Console.WriteLine(e.ToString());
                        AbpfLogUtil.LogError(e, null);
                        throw;
                    }
                }

                // アンマネージド リソース (アンマネージド オブジェクト) を解放し、ファイナライザーをオーバーライドします
                // 大きなフィールドを null に設定します
                this.disposedValue = true;
            }
        }
    }
}
