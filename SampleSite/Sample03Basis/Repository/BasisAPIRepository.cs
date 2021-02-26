// <copyright file="BasisAPITransactionRepository.cs" company="Above Beyond Project Framework">
// Copyright (c) Above Beyond Project Framework. All rights reserved.
// </copyright>

namespace Sample03Basis.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Text;
    using AbpfBase.Repository;
    using AbpfUtil.Models;
    using AbpfUtil.Util;
    using Microsoft.Data.SqlClient;
    using Sample01Base.Repository;
    using Sample03Basis.Models;

    /// <summary>
    /// 基礎テーブルAPIリポジトリ
    /// </summary>
    public class BasisAPIRepository : SampleRepositoryBase
    {
        // SQLConnection
        private readonly SqlConnection con;

        /// <summary>
        /// Initializes a new instance of the <see cref="BasisAPIRepository"/> class.
        /// </summary>
        /// <param name="con">DB接続</param>
        public BasisAPIRepository(SqlConnection con)
            : base(con)
        {
            this.con = con;
        }

        /// <summary>
        /// GetBasis
        /// </summary>
        /// <remarks>
        /// 検索結果を返す（トランザクションなし）
        /// </remarks>
        /// <param name="jobNo">ジョブNO</param>
        /// <param name="userInfo">ユーザー情報</param>
        /// <returns>基礎テーブルデータリスト</returns>
        public List<BasisTableData> GetBasis(string jobNo, UserInfo userInfo)
        {
            List<BasisTableData> results = new List<BasisTableData>();
            List<SqlParameter> paramList = new List<SqlParameter>();

            StringBuilder sqlstr = new StringBuilder(@$"
SELECT JOB_NO, EST_NO, EST_NO_EDA, BKG_DATE, BK_MEMO, WORK_USER_ID, DEST_CD
FROM C_BASIS
");

            if (string.IsNullOrEmpty(jobNo) == false && jobNo.Trim().Length > 0)
            {
                sqlstr.AppendLine(@$"
WHERE JOB_NO = @JOB_NO
");
                paramList.Add(AbpfDBUtil.GetSqlParameter("JOB_NO", jobNo));
            }

            using (SqlCommand com = new SqlCommand(sqlstr.ToString(), this.con))
            {
                if (paramList.Count > 0)
                {
                    com.Parameters.AddRange(paramList.ToArray());
                }

                // 検索結果をList<BasisTableData>に変換
                var dre = new AbpfDataReaderEnumerable<BasisTableData>(com, userInfo);
                foreach (BasisTableData row in dre.AsEnumerable())
                {
                    results.Add(row);
                }
            }

            return results;
        }

        /// <summary>
        /// InsertBasis
        /// </summary>
        /// <remarks>
        /// 新規登録する
        /// </remarks>
        /// <param name="basisCargoData">基礎テーブルデータ</param>
        /// <param name="trn">トランザクション</param>
        /// <param name="userInfo">ユーザー情報</param>
        /// <returns>処理件数</returns>
        public string InsertBasis(BasisCargoData basisCargoData, SqlTransaction trn, UserInfo userInfo)
        {
            try
            {
                string result = string.Empty;

                List<SqlParameter> paramList = new List<SqlParameter>();

                // SQL文
                StringBuilder sqlstr = new StringBuilder(@$"
INSERT INTO C_BASIS
(JOB_NO, EST_NO, EST_NO_EDA)
VALUES
(@JOB_NO, @EST_NO, @EST_NO_EDA)
");

                // パラメータ
                paramList.Add(AbpfDBUtil.GetSqlParameter("JOB_NO", basisCargoData.JobNo));
                paramList.Add(AbpfDBUtil.GetSqlParameter("EST_NO", basisCargoData.EstNo));
                paramList.Add(AbpfDBUtil.GetSqlParameter("EST_NO_EDA", basisCargoData.EstNoEda));

                using (SqlCommand com = new SqlCommand(sqlstr.ToString(), this.con, trn))
                {
                    if (paramList.Count > 0)
                    {
                        com.Parameters.AddRange(paramList.ToArray());
                    }

                    // SQL実行
                    result = this.AbpfExecuteNonQuery(com, userInfo).ToString();
                }

                // 明細行の追加
                foreach (CargoTableData item in basisCargoData.ItemsAdded)
                {
                    item.JobNo = basisCargoData.JobNo;
                    this.InsertCargo(item, trn, userInfo);
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
        /// UpdateBasis
        /// </summary>
        /// <remarks>
        /// 更新する
        /// </remarks>
        /// <param name="basisCargoData">基礎テーブルデータ</param>
        /// <param name="trn">トランザクション</param>
        /// <param name="userInfo">ユーザー情報</param>
        /// <returns>処理件数</returns>
        public string UpdateBasis(BasisCargoData basisCargoData, SqlTransaction trn, UserInfo userInfo)
        {
            try
            {
                string result = string.Empty;

                List<SqlParameter> paramList = new List<SqlParameter>();

                // SQL文
                StringBuilder sqlstr = new StringBuilder(@$"
UPDATE C_BASIS
SET EST_NO = @EST_NO
, EST_NO_EDA = @EST_NO_EDA
WHERE JOB_NO = @JOB_NO
");

                // パラメータ
                paramList.Add(AbpfDBUtil.GetSqlParameter("JOB_NO", basisCargoData.JobNo));
                paramList.Add(AbpfDBUtil.GetSqlParameter("EST_NO", basisCargoData.EstNo));
                paramList.Add(AbpfDBUtil.GetSqlParameter("EST_NO_EDA", basisCargoData.EstNoEda));

                using (SqlCommand com = new SqlCommand(sqlstr.ToString(), this.con, trn))
                {
                    if (paramList.Count > 0)
                    {
                        com.Parameters.AddRange(paramList.ToArray());
                    }

                    // SQL実行
                    result = this.AbpfExecuteNonQuery(com, userInfo).ToString();
                }

                // 明細行の更新
                foreach (CargoTableData item in basisCargoData.ItemsEdited)
                {
                    item.JobNo = basisCargoData.JobNo;
                    this.UpdateCargo(item, trn, userInfo);
                }

                // 明細行の追加
                foreach (CargoTableData item in basisCargoData.ItemsAdded)
                {
                    item.JobNo = basisCargoData.JobNo;
                    this.InsertCargo(item, trn, userInfo);
                }

                // 明細行の削除
                foreach (CargoTableData item in basisCargoData.ItemsRemoved)
                {
                    item.JobNo = basisCargoData.JobNo;
                    this.DeleteCargo(item.JobNo, item.CargoSeq.ToString(), trn, userInfo);
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
        /// DeleteBasis
        /// </summary>
        /// <remarks>
        /// 削除する
        /// </remarks>
        /// <param name="jobNo">ジョブNO</param>
        /// <param name="trn">トランザクション</param>
        /// <param name="userInfo">ユーザー情報</param>
        /// <returns>件数</returns>
        public string DeleteBasis(string jobNo, SqlTransaction trn, UserInfo userInfo)
        {
            try
            {
                string result = string.Empty;

                List<SqlParameter> paramList = new List<SqlParameter>();

                // SQL文
                StringBuilder sqlstr = new StringBuilder(@$"
DELETE FROM C_BASIS
WHERE JOB_NO = @JOB_NO
");

                // パラメータ
                paramList.Add(AbpfDBUtil.GetSqlParameter("JOB_NO", jobNo));

                using (SqlCommand com = new SqlCommand(sqlstr.ToString(), this.con, trn))
                {
                    if (paramList.Count > 0)
                    {
                        com.Parameters.AddRange(paramList.ToArray());
                    }

                    // SQL実行
                    result = this.AbpfExecuteNonQuery(com, userInfo).ToString();
                }

                // 明細行全削除
                this.DeleteCargo(jobNo, string.Empty, trn, userInfo);

                return result;
            }
            catch (Exception ex)
            {
                AbpfLogUtil.LogError(ex, userInfo);
                throw;
            }
        }

        /// <summary>
        /// StoredProcedureTest
        /// </summary>
        /// <remarks>
        /// 一括更新する
        /// ストアドプロシージャでSYS_UPD_DATEを一括更新する
        /// </remarks>
        /// <param name="jobNo">ジョブNO</param>
        /// <param name="trn">トランザクション</param>
        /// <param name="userInfo">ユーザー情報</param>
        /// <returns>件数</returns>
        public string StoredProcedureTest(string jobNo, SqlTransaction trn, UserInfo userInfo)
        {
            try
            {
                string result = string.Empty;

                using (SqlCommand com = new SqlCommand("TEST_UPDATE_UPDDATE", this.con, trn))
                {
                    // プロシージャのパラメータ指定
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("JobNo", jobNo);

                    // プロシージャの戻り値パラメータ
                    com.Parameters.Add("ReturnValue", System.Data.SqlDbType.Int);
                    com.Parameters["ReturnValue"].Direction = System.Data.ParameterDirection.ReturnValue;

                    // SQL実行
                    result = this.AbpfExecuteNonQuery(com, userInfo).ToString();

                    result = com.Parameters["ReturnValue"].Value.ToString();
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
        /// GetCargo
        /// </summary>
        /// <remarks>
        /// JOBNOで検索結果を返す（トランザクションなし）
        /// </remarks>
        /// <param name="jobNo">ジョブNO</param>
        /// <param name="userInfo">ユーザー情報</param>
        /// <returns>貨物テーブルデータリスト</returns>
        public List<CargoTableData> GetCargo(string jobNo, UserInfo userInfo)
        {
            List<CargoTableData> results = new List<CargoTableData>();
            List<SqlParameter> paramList = new List<SqlParameter>();

            // SQL文
            StringBuilder sqlstr = new StringBuilder(@$"
SELECT JOB_NO, CARGO_SEQ, NET_WEIGHT, PG_KBN, FPOINT_UNIT_KBN, NOS_KBN, MP_KBN, SYS_ENT_DATE, SYS_ENT_TIME
FROM C_CARGO
WHERE JOB_NO     = @JOB_NO
");

            // パラメータ
            paramList.Add(AbpfDBUtil.GetSqlParameter("JOB_NO", jobNo));

            using (SqlCommand com = new SqlCommand(sqlstr.ToString(), this.con))
            {
                if (paramList.Count > 0)
                {
                    com.Parameters.AddRange(paramList.ToArray());
                }

                // 検索結果をList<CargoTableData>に変換
                var dre = new AbpfDataReaderEnumerable<CargoTableData>(com, userInfo);
                foreach (CargoTableData row in dre.AsEnumerable())
                {
                    results.Add(row);
                }
            }

            return results;
        }

        /// <summary>
        /// CheckCargoExists
        /// </summary>
        /// <remarks>
        /// 存在チェック
        /// </remarks>
        /// <param name="jobNo">ジョブNO</param>
        /// <param name="cargoSeq">貨物SEQ</param>
        /// <param name="trn">トランザクション</param>
        /// <returns>true：存在、false：存在しない </returns>
        public bool CheckCargoExists(string jobNo, string cargoSeq, SqlTransaction trn, UserInfo userInfo)
        {
            List<CargoTableData> results = new List<CargoTableData>();
            List<SqlParameter> paramList = new List<SqlParameter>();

            // SQL文
            StringBuilder sqlstr = new StringBuilder(@$"
SELECT COUNT(*) AS CNT
FROM C_CARGO
WHERE JOB_NO    = @JOB_NO
  AND CARGO_SEQ = @CARGO_SEQ
");

            // パラメータ
            paramList.Add(AbpfDBUtil.GetSqlParameter("JOB_NO", jobNo));
            paramList.Add(AbpfDBUtil.GetSqlParameter("CARGO_SEQ", cargoSeq));

            using SqlCommand com = new SqlCommand(sqlstr.ToString(), this.con);
            com.Parameters.AddRange(paramList.ToArray());

            using var sdr = com.ExecuteReader();
            if (sdr.HasRows == false)
            {
                return false;
            }

            sdr.Read();
            return int.Parse(sdr.GetName(0)) > 0;
        }

        /// <summary>
        /// InsertCargo
        /// </summary>
        /// <remarks>
        /// 新規登録する/キーが存在したら更新する
        /// </remarks>
        /// <param name="cargoTableData">貨物テーブルデータ</param>
        /// <param name="trn">トランザクション</param>
        /// <param name="userInfo">ユーザー情報</param>
        /// <returns>処理件数</returns>
        public string InsertCargo(CargoTableData cargoTableData, SqlTransaction trn, UserInfo userInfo)
        {
            try
            {
                string result = string.Empty;

                // キーが存在したら更新する
                if (this.CheckCargoExists(cargoTableData.JobNo, cargoTableData.CargoSeq.ToString(), trn, userInfo))
                {
                    return this.UpdateCargo(cargoTableData, trn, userInfo);
                }

                List<SqlParameter> paramList = new List<SqlParameter>();

                // SQL文
                StringBuilder sqlstr = new StringBuilder(@$"
INSERT INTO C_CARGO
(JOB_NO, CARGO_SEQ, NET_WEIGHT, PG_KBN, FPOINT_UNIT_KBN, NOS_KBN, MP_KBN, SYS_ENT_DATE, SYS_ENT_TIME)
VALUES
(@JOB_NO, @CARGO_SEQ, @NET_WEIGHT, @PG_KBN, @FPOINT_UNIT_KBN, @NOS_KBN, @MP_KBN, @SYS_ENT_DATE, @SYS_ENT_TIME)
");

                // パラメータ
                paramList.Add(AbpfDBUtil.GetSqlParameter("JOB_NO", cargoTableData.JobNo));
                paramList.Add(AbpfDBUtil.GetSqlParameter("CARGO_SEQ", cargoTableData.CargoSeq));
                paramList.Add(AbpfDBUtil.GetSqlParameter("NET_WEIGHT", cargoTableData.NetWeight));
                paramList.Add(AbpfDBUtil.GetSqlParameter("PG_KBN", cargoTableData.PgKbn));
                paramList.Add(AbpfDBUtil.GetSqlParameter("FPOINT_UNIT_KBN", cargoTableData.FpointUnitKbn));
                paramList.Add(AbpfDBUtil.GetSqlParameter("NOS_KBN", cargoTableData.NosKbn));
                paramList.Add(AbpfDBUtil.GetSqlParameter("MP_KBN", cargoTableData.MpKbn));
                paramList.Add(AbpfDBUtil.GetSqlParameter("SYS_ENT_DATE", cargoTableData.SysEntDate));
                paramList.Add(AbpfDBUtil.GetSqlParameter("SYS_ENT_TIME", cargoTableData.SysEntTime));

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
        /// UpdateCargo
        /// </summary>
        /// <remarks>
        /// 更新する
        /// </remarks>
        /// <param name="cargoTableData">貨物テーブルデータ</param>
        /// <param name="trn">トランザクション</param>
        /// <param name="userInfo">ユーザー情報</param>
        /// <returns>処理件数</returns>
        public string UpdateCargo(CargoTableData cargoTableData, SqlTransaction trn, UserInfo userInfo)
        {
            try
            {
                string result = string.Empty;

                List<SqlParameter> paramList = new List<SqlParameter>();

                // SQL文
                StringBuilder sqlstr = new StringBuilder(@$"
UPDATE C_CARGO
SET NET_WEIGHT = @NET_WEIGHT
, PG_KBN = @PG_KBN
, FPOINT_UNIT_KBN = @FPOINT_UNIT_KBN
, NOS_KBN = @NOS_KBN
, MP_KBN = @MP_KBN
, SYS_ENT_DATE = @SYS_ENT_DATE
, SYS_ENT_TIME = @SYS_ENT_TIME
WHERE JOB_NO    = @JOB_NO
AND   CARGO_SEQ = @CARGO_SEQ
");

                // パラメータ
                paramList.Add(AbpfDBUtil.GetSqlParameter("JOB_NO", cargoTableData.JobNo));
                paramList.Add(AbpfDBUtil.GetSqlParameter("CARGO_SEQ", cargoTableData.CargoSeq));
                paramList.Add(AbpfDBUtil.GetSqlParameter("NET_WEIGHT", cargoTableData.NetWeight));
                paramList.Add(AbpfDBUtil.GetSqlParameter("PG_KBN", cargoTableData.PgKbn));
                paramList.Add(AbpfDBUtil.GetSqlParameter("FPOINT_UNIT_KBN", cargoTableData.FpointUnitKbn));
                paramList.Add(AbpfDBUtil.GetSqlParameter("NOS_KBN", cargoTableData.NosKbn));
                paramList.Add(AbpfDBUtil.GetSqlParameter("MP_KBN", cargoTableData.MpKbn));
                paramList.Add(AbpfDBUtil.GetSqlParameter("SYS_ENT_DATE", cargoTableData.SysEntDate));
                paramList.Add(AbpfDBUtil.GetSqlParameter("SYS_ENT_TIME", cargoTableData.SysEntTime));

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
        /// DeleteCargo
        /// </summary>
        /// <remarks>
        /// 削除する
        /// </remarks>
        /// <param name="jobNo">ジョブNO</param>
        /// <param name="cargoSeq">貨物SEQ</param>
        /// <param name="trn">トランザクション</param>
        /// <param name="userInfo">ユーザー情報</param>
        /// <returns>件数</returns>
        public string DeleteCargo(string jobNo, string cargoSeq, SqlTransaction trn, UserInfo userInfo)
        {
            try
            {
                string result = string.Empty;

                List<SqlParameter> paramList = new List<SqlParameter>();

                // SQL文
                StringBuilder sqlstr = new StringBuilder(@$"
DELETE C_CARGO
WHERE JOB_NO    = @JOB_NO
");

                // パラメータ
                paramList.Add(AbpfDBUtil.GetSqlParameter("JOB_NO", jobNo));

                // 第二パラメータがなければ第一パラメータで削除
                if (string.IsNullOrEmpty(cargoSeq) == false && cargoSeq.Trim().Length > 0)
                {
                    sqlstr.Append(@$"
AND   CARGO_SEQ = @CARGO_SEQ
");
                    paramList.Add(AbpfDBUtil.GetSqlParameter("CARGO_SEQ", cargoSeq));
                }

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
