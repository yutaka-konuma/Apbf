// <copyright file="AbpfChatHubServiceBase.cs" company="Above Beyond Project Framework">
// Copyright (c) Above Beyond Project Framework. All rights reserved.
// </copyright>

namespace AbpfProto.Service
{
    using System;
    using Microsoft.Data.SqlClient;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Abpf基盤サービス
    /// </summary>
    public class AbpfChatHubServiceBase : IDisposable
    {
        protected SqlConnection sqlConnection;
        private bool disposedValue;
        private ILoggerFactory loggerFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="AbpfChatHubServiceBase"/> class.
        /// </summary>
        /// <param name="loggerFactory">a.</param>
        public AbpfChatHubServiceBase(ILoggerFactory loggerFactory)
        {
            try
            {
                this.loggerFactory = loggerFactory;
                this.sqlConnection = new SqlConnection("Server=linabpfprotosql.database.windows.net;Database=linabpfprotodb;User ID=abpf;Password=Zaq12wsx;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
                this.sqlConnection.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

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
                        Console.WriteLine(e.ToString());
                    }
                }

                // アンマネージド リソース (アンマネージド オブジェクト) を解放し、ファイナライザーをオーバーライドします
                // 大きなフィールドを null に設定します
                this.disposedValue = true;
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            // このコードを変更しないでください。クリーンアップ コードを 'Dispose(bool disposing)' メソッドに記述します
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
