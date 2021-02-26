// <copyright file="AbpfRepositoryBase.cs" company="Above Beyond Project Framework">
// Copyright (c) Above Beyond Project Framework. All rights reserved.
// </copyright>

namespace AbpfBase.Repository
{
    using System.Runtime.CompilerServices;
    using AbpfUtil.Models;
    using AbpfUtil.Util;
    using Microsoft.Data.SqlClient;

    /// <summary>
    /// Abpf基盤リポジトリ
    /// </summary>
    public class AbpfRepositoryBase
    {
        // SQLConnection
        private readonly SqlConnection con;

        /// <summary>
        /// Initializes a new instance of the <see cref="AbpfRepositoryBase"/> class.
        /// </summary>
        /// <param name="con">DB接続</param>
        public AbpfRepositoryBase(SqlConnection con)
        {
            this.con = con;
        }

        /// <summary>
        /// 通常のExecuteReader+ログ出力
        /// </summary>
        /// <param name="com">SQLConnection</param>/
        /// <param name="userInfo">ユーザー情報</param>/
        /// <param name="memberName">メンバー名（呼び出し側は指定しない）</param>
        /// <param name="filePath">ファイル名（呼び出し側は指定しない）</param>
        /// <param name="lineNumber">行番号（呼び出し側は指定しない）</param>
        /// <returns>DataReaderを返す</returns>
        public SqlDataReader AbpfExecuteReader(
            SqlCommand com,
            UserInfo userInfo,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = -1)
        {
            // SQL実行ログをここで出力
            AbpfLogUtil.LogDebug(com, userInfo, memberName, filePath, lineNumber);

            return com.ExecuteReader();
        }

        /// <summary>
        /// 通常のExecuteNonQuery+ログ出力
        /// </summary>
        /// <param name="com">SQLConnection</param>/
        /// <param name="userInfo">ユーザー情報</param>/
        /// <param name="memberName">メンバー名（呼び出し側は指定しない）</param>
        /// <param name="filePath">ファイル名（呼び出し側は指定しない）</param>
        /// <param name="lineNumber">行番号（呼び出し側は指定しない）</param>
        /// <returns>結果（件数）を返す</returns>
        public int AbpfExecuteNonQuery(
            SqlCommand com,
            UserInfo userInfo,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = -1)
        {
            // SQL実行ログをここで出力
            AbpfLogUtil.LogDebug(com, userInfo, memberName, filePath, lineNumber);
        
            return com.ExecuteNonQuery();
        }
    }
}
