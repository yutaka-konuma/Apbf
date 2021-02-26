// <copyright file="GoodsMasterAPITransactionRepository.cs" company="Above Beyond Project Framework">
// Copyright (c) Above Beyond Project Framework. All rights reserved.
// </copyright>

namespace Sample02GoodsMaster.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using AbpfBase.Repository;
    using AbpfUtil.Models;
    using AbpfUtil.Util;
    using Microsoft.Data.SqlClient;
    using Sample01Base.Repository;
    using Sample02GoodsMaster.Models;

    /// <summary>
    /// 商品マスタAPIリポジトリ
    /// </summary>
    public class GoodsMasterAPIRepository : SampleRepositoryBase
    {
        // SQLConnection
        private readonly SqlConnection con;

        // テーブル名
        private string tableName = "M_GOODS";

        /// <summary>
        /// Initializes a new instance of the <see cref="GoodsMasterAPIRepository"/> class.
        /// </summary>
        /// <param name="con">DB接続</param>
        public GoodsMasterAPIRepository(SqlConnection con)
            : base(con)
        {
            this.con = con;
        }

        /// <summary>
        /// Get
        /// </summary>
        /// <remarks>
        /// 検索結果を返す
        /// </remarks>
        /// <param name="goodsCd">商品コード</param>
        /// <param name="userInfo">ユーザー情報</param>
        /// <returns>商品マスタデータリスト</returns>
        public List<GoodsMasterData> Get(string goodsCd, UserInfo userInfo)
        {
            List<GoodsMasterData> results = new List<GoodsMasterData>();
            List<SqlParameter> paramList = new List<SqlParameter>();

            // SQL文
            StringBuilder sqlstr = new StringBuilder(@$"
SELECT GOODS_CD, GOODS_ENM, NET_WEIGHT, PG_KBN, FPOINT_UNIT_KBN, NOS_FLG, MP_FLG, SYS_ENT_DATE
FROM " + this.tableName + @$"
");

            // パラメータがなければ全件
            if (string.IsNullOrEmpty(goodsCd) == false && goodsCd.Trim().Length > 0)
            {
                sqlstr.Append(@$"
WHERE GOODS_CD = @GOODS_CD
");

                // パラメータ
                paramList.Add(AbpfDBUtil.GetSqlParameter("GOODS_CD", goodsCd));
            }

            using (SqlCommand com = new SqlCommand(sqlstr.ToString(), this.con))
            {
                if (paramList.Count > 0)
                {
                    com.Parameters.AddRange(paramList.ToArray());
                }

                // 検索結果をList<GoodsMasterData>に変換
                var dre = new AbpfDataReaderEnumerable<GoodsMasterData>(com, userInfo);
                foreach (GoodsMasterData row in dre.AsEnumerable())
                {
                    results.Add(row);
                }
            }

            return results;
        }

        /// <summary>
        /// Post(Insert)
        /// </summary>
        /// <remarks>
        /// 新規登録する
        /// </remarks>
        /// <param name="form">商品マスタデータ</param>
        /// <param name="trn">トランザクション</param>
        /// <param name="userInfo">ユーザー情報</param>
        /// <returns>処理件数</returns>
        public string Post(GoodsMasterData form, SqlTransaction trn, UserInfo userInfo)
        {
            try
            {
                string result = string.Empty;

                List<SqlParameter> paramList = new List<SqlParameter>();

                // SQL文
                StringBuilder sqlstr = new StringBuilder(@$"
INSERT INTO " + this.tableName + @$"
(GOODS_CD, GOODS_ENM, NET_WEIGHT, PG_KBN, FPOINT_UNIT_KBN, NOS_FLG, MP_FLG, SYS_ENT_DATE)
VALUES
(@GOODS_CD, @GOODS_ENM, @NET_WEIGHT, @PG_KBN, @FPOINT_UNIT_KBN, @NOS_FLG, @MP_FLG, @SYS_ENT_DATE)
");

                // パラメータ
                paramList.Add(AbpfDBUtil.GetSqlParameter("GOODS_CD", form.GoodsCd));
                paramList.Add(AbpfDBUtil.GetSqlParameter("GOODS_ENM", form.GoodsEnm));
                paramList.Add(AbpfDBUtil.GetSqlParameter("NET_WEIGHT", form.NetWeight));
                paramList.Add(AbpfDBUtil.GetSqlParameter("PG_KBN", form.PgKbn));
                paramList.Add(AbpfDBUtil.GetSqlParameter("FPOINT_UNIT_KBN", form.FpointUnitKbn));
                paramList.Add(AbpfDBUtil.GetSqlParameter("NOS_FLG", form.NosFlg));
                paramList.Add(AbpfDBUtil.GetSqlParameter("MP_FLG", form.MpFlg));
                paramList.Add(AbpfDBUtil.GetSqlParameter("SYS_ENT_DATE", form.SysEntDate));

                using (SqlCommand com = new SqlCommand(sqlstr.ToString(), this.con, trn))
                {
                    if (paramList.Count > 0)
                    {
                        com.Parameters.AddRange(paramList.ToArray());
                    }

                    // SQL実行
                    result = this.AbpfExecuteNonQuery(com, userInfo).ToString();
                }

                return result;
            }
            catch (Exception ex)
            {
                AbpfLogUtil.LogError(ex, userInfo);
                throw;
            }
        }

        /// <summary>
        /// Put(Update)
        /// </summary>
        /// <remarks>
        /// 更新する
        /// </remarks>
        /// <param name="form">商品マスタデータ</param>
        /// <param name="trn">トランザクション</param>
        /// <param name="userInfo">ユーザー情報</param>
        /// <returns>処理件数</returns>
        public string Put(GoodsMasterData form, SqlTransaction trn, UserInfo userInfo)
        {
            try
            {
                string result = string.Empty;

                List<SqlParameter> paramList = new List<SqlParameter>();

                // SQL文
                StringBuilder sqlstr = new StringBuilder(@$"
UPDATE " + this.tableName + @$"
SET GOODS_ENM = @GOODS_ENM
, NET_WEIGHT = @NET_WEIGHT
, PG_KBN = @PG_KBN
, FPOINT_UNIT_KBN = @FPOINT_UNIT_KBN
, NOS_FLG = @NOS_FLG
, MP_FLG = @MP_FLG
, SYS_ENT_DATE = @SYS_ENT_DATE
WHERE GOODS_CD = @GOODS_CD
");

                // パラメータ
                paramList.Add(AbpfDBUtil.GetSqlParameter("GOODS_CD", form.GoodsCd));
                paramList.Add(AbpfDBUtil.GetSqlParameter("GOODS_ENM", form.GoodsEnm));
                paramList.Add(AbpfDBUtil.GetSqlParameter("NET_WEIGHT", form.NetWeight));
                paramList.Add(AbpfDBUtil.GetSqlParameter("PG_KBN", form.PgKbn));
                paramList.Add(AbpfDBUtil.GetSqlParameter("FPOINT_UNIT_KBN", form.FpointUnitKbn));
                paramList.Add(AbpfDBUtil.GetSqlParameter("NOS_FLG", form.NosFlg));
                paramList.Add(AbpfDBUtil.GetSqlParameter("MP_FLG", form.MpFlg));
                paramList.Add(AbpfDBUtil.GetSqlParameter("SYS_ENT_DATE", form.SysEntDate));

                using (SqlCommand com = new SqlCommand(sqlstr.ToString(), this.con, trn))
                {
                    if (paramList.Count > 0)
                    {
                        com.Parameters.AddRange(paramList.ToArray());
                    }

                    // SQL実行
                    result = this.AbpfExecuteNonQuery(com, userInfo).ToString();
                }

                return result;
            }
            catch (Exception ex)
            {
                AbpfLogUtil.LogError(ex, userInfo);
                throw;
            }
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <remarks>
        /// 削除する
        /// </remarks>
        /// <param name="goodsCd">商品コード</param>
        /// <param name="trn">トランザクション</param>
        /// <param name="userInfo">ユーザー情報</param>
        /// <returns>件数</returns>
        public string Delete(string goodsCd, SqlTransaction trn, UserInfo userInfo)
        {
            try
            {
                string result = string.Empty;

                List<SqlParameter> paramList = new List<SqlParameter>();

                // SQL文
                StringBuilder sqlstr = new StringBuilder(@$"
DELETE FROM " + this.tableName + @$"
WHERE GOODS_CD = @GOODS_CD
");

                // パラメータ
                paramList.Add(AbpfDBUtil.GetSqlParameter("GOODS_CD", goodsCd));

                using (SqlCommand com = new SqlCommand(sqlstr.ToString(), this.con, trn))
                {
                    if (paramList.Count > 0)
                    {
                        com.Parameters.AddRange(paramList.ToArray());
                    }

                    // SQL実行
                    result = this.AbpfExecuteNonQuery(com, userInfo).ToString();
                }

                return result;
            }
            catch (Exception ex)
            {
                AbpfLogUtil.LogError(ex, userInfo);
                throw;
            }
        }
    }
}
