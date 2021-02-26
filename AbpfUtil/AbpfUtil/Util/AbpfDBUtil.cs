// <copyright file="AbpfDBUtil.cs" company="Above Beyond Project Framework">
// Copyright (c) Above Beyond Project Framework. All rights reserved.
// </copyright>

namespace AbpfUtil.Util
{
    using System.Data;
    using Microsoft.Data.SqlClient;

    /// <summary>
    /// DB関連処理
    /// </summary>
    public static class AbpfDBUtil
    {
        /// <summary>
        /// パラメータ作成(string)
        /// </summary>
        /// <param name="fieldName">項目名</param>/
        /// <param name="data">値</param>
        /// <returns>新しいSqlParameterを返す</returns>
        public static SqlParameter GetSqlParameter(
            string fieldName,
            string data)
        {
            SqlParameter result = new SqlParameter(fieldName, SqlDbType.VarChar);
            if (data == null)
            {
                result.Value = string.Empty;
                return result;
            }

            result.Value = data;
            return result;
        }

        /// <summary>
        /// パラメータ作成(int)
        /// </summary>
        /// <param name="fieldName">項目名</param>/
        /// <param name="data">値</param>
        /// <returns>新しいSqlParameterを返す</returns>
        public static SqlParameter GetSqlParameter(
            string fieldName,
            int data)
        {
            SqlParameter result = new SqlParameter(fieldName, SqlDbType.Decimal);
            result.Value = data;
            return result;
        }

        /// <summary>
        /// パラメータ作成(long)
        /// </summary>
        /// <param name="fieldName">項目名</param>/
        /// <param name="data">値</param>
        /// <returns>新しいSqlParameterを返す</returns>
        public static SqlParameter GetSqlParameter(
            string fieldName,
            long data)
        {
            SqlParameter result = new SqlParameter(fieldName, SqlDbType.Decimal);
            result.Value = data;
            return result;
        }

        /// <summary>
        /// パラメータ作成(float)
        /// </summary>
        /// <param name="fieldName">項目名</param>/
        /// <param name="data">値</param>
        /// <returns>新しいSqlParameterを返す</returns>
        public static SqlParameter GetSqlParameter(
            string fieldName,
            float data)
        {
            SqlParameter result = new SqlParameter(fieldName, SqlDbType.Decimal);
            result.Value = data;
            return result;
        }

        /// <summary>
        /// パラメータ作成(double)
        /// </summary>
        /// <param name="fieldName">項目名</param>/
        /// <param name="data">値</param>
        /// <returns>新しいSqlParameterを返す</returns>
        public static SqlParameter GetSqlParameter(
            string fieldName,
            double data)
        {
            SqlParameter result = new SqlParameter(fieldName, SqlDbType.Decimal);
            result.Value = data;
            return result;
        }

        /// <summary>
        /// パラメータ作成(decimal)
        /// </summary>
        /// <param name="fieldName">項目名</param>/
        /// <param name="data">値</param>
        /// <returns>新しいSqlParameterを返す</returns>
        public static SqlParameter GetSqlParameter(
            string fieldName,
            decimal data)
        {
            SqlParameter result = new SqlParameter(fieldName, SqlDbType.Decimal);
            result.Value = data;
            return result;
        }
    }
}
