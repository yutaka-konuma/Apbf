// <copyright file="AbpfDBConnUtil.cs" company="Above Beyond Project Framework">
// Copyright (c) Above Beyond Project Framework. All rights reserved.
// </copyright>

namespace AbpfUtil.Util
{
    using System;
    using Microsoft.Data.SqlClient;

    /// <summary>
    /// DB接続処理
    /// </summary>
    public static class AbpfDBConnUtil
    {
        /// <summary>
        /// 解放処理
        /// </summary>
        /// <param name="con">SqlConnection</param>
        public static void Dispose(SqlConnection con)
        {
            try
            {
                con.Dispose();
            }
            catch (Exception)
            {
                // 何もしない
            }
        }

        /// <summary>
        /// DB接続処理
        /// </summary>
        /// <param name="connectingString">接続文字列</param>
        /// <returns>SqlConnection</returns>
        public static SqlConnection ConnectionOpen(string connectingString)
        {
            SqlConnection con = new SqlConnection(connectingString);
            con.Open();
            return con;
        }

        /// <summary>
        /// クローズ処理
        /// </summary>
        /// <param name="con">SqlConnection</param>
        public static void ConnectionClose(SqlConnection con)
        {
            try
            {
                con.Close();
            }
            catch (Exception)
            {
                // 何もしない
            }
        }

        /// <summary>
        /// トランザクション開始
        /// </summary>
        /// <param name="con">SqlConnection</param>
        /// <returns>SqlTransaction</returns>
        public static SqlTransaction BeginTransaction(SqlConnection con)
        {
            return con.BeginTransaction();
        }

        /// <summary>
        /// コミット
        /// </summary>
        /// <param name="trn">SqlTransaction</param>
        public static void CommitTransaction(SqlTransaction trn)
        {
            try
            {
                trn.Commit();
            }
            finally
            {
                // 破棄
                trn.Dispose();
            }
        }

        /// <summary>
        /// ロールバック
        /// </summary>
        /// <param name="trn">SqlTransaction</param>
        public static void RollbackTransaction(SqlTransaction trn)
        {
            try
            {
                trn.Rollback();
            }
            finally
            {
                // 破棄
                trn.Dispose();
            }
        }
    }
}
