// <copyright file="BasisAPITransactionService.cs" company="Above Beyond Project Framework">
// Copyright (c) Above Beyond Project Framework. All rights reserved.
// </copyright>

namespace Sample03Basis.Service
{
    using System;
    using System.Collections.Generic;
    using AbpfUtil.Models;
    using Microsoft.Data.SqlClient;
    using Microsoft.Extensions.Logging;
    using Sample01Base.Service;
    using Sample03Basis.Models;
    using Sample03Basis.Repository;

    /// <summary>
    /// 基礎テーブルAPIサービス
    /// </summary>
    public class BasisAPIService : SampleServiceBase
    {
        // 標準ロガー
        private readonly ILoggerFactory logger;

        // 基礎テーブルAPIレポジトリ
        private BasisAPIRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="BasisAPIService"/> class.
        /// </summary>
        /// <param name="logger">標準ロガー</param>
        public BasisAPIService(ILoggerFactory logger)
            : base(logger)
        {
            this.logger = logger;
            this.repository = new BasisAPIRepository(this.sqlConnection);
        }

        /// <summary>
        /// GetBasis
        /// </summary>
        /// <remarks>
        /// 検索結果を返す
        /// </remarks>
        /// <param name="jobNo">ジョブNO</param>
        /// <param name="userInfo">ユーザー情報</param>
        /// <returns>基礎テーブルデータリスト</returns>
        public List<BasisTableData> GetBasis(string jobNo, UserInfo userInfo)
        {
            List<BasisTableData> result;

            try
            {
                result = this.repository.GetBasis(jobNo, userInfo);
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        /// <summary>
        /// GetCargo
        /// </summary>
        /// <remarks>
        /// 検索結果を返す
        /// </remarks>
        /// <param name="jobNo">ジョブNO</param>
        /// <param name="cargoSeq">貨物SEQ</param>
        /// <param name="userInfo">ユーザー情報</param>
        /// <returns>貨物テーブルデータリスト</returns>
        public List<CargoTableData> GetCargo(string jobNo, UserInfo userInfo)
        {
            List<CargoTableData> result;

            try
            {
                result = this.repository.GetCargo(jobNo, userInfo);
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        /// <summary>
        /// InsertBasis
        /// </summary>
        /// <remarks>
        /// 新規登録する
        /// トランザクション管理する
        /// </remarks>
        /// <param name="basisCargoData">基礎テーブルデータ</param>
        /// <param name="userInfo">ユーザー情報</param>
        /// <returns>処理件数</returns>
        public string InsertBasis(BasisCargoData basisCargoData, UserInfo userInfo)
        {
            string result = string.Empty;

            SqlTransaction trn = this.sqlConnection.BeginTransaction();
            try
            {
                result = this.repository.InsertBasis(basisCargoData, trn, userInfo);
                trn.Commit();
            }
            catch (Exception)
            {
                trn.Rollback();
                throw;
            }

            return result;
        }

        /// <summary>
        /// UpdateBasis
        /// </summary>
        /// <remarks>
        /// 更新する
        /// </remarks>
        /// <param name="basisCargoData">基礎テーブルデータ</param>
        /// <param name="userInfo">ユーザー情報</param>
        /// <returns>処理件数</returns>
        public string UpdateBasis(BasisCargoData basisCargoData, UserInfo userInfo)
        {
            string result = string.Empty;

            SqlTransaction trn = this.sqlConnection.BeginTransaction();
            try
            {
                result = this.repository.UpdateBasis(basisCargoData, trn, userInfo);
                trn.Commit();
            }
            catch (Exception)
            {
                trn.Rollback();
                throw;
            }

            return result;
        }

        /// <summary>
        /// DeleteBasis
        /// </summary>
        /// <remarks>
        /// 削除する
        /// </remarks>
        /// <param name="jobNo">ジョブNO</param>
        /// <param name="userInfo">ユーザー情報</param>
        /// <returns>件数</returns>
        public string DeleteBasis(string jobNo, UserInfo userInfo)
        {
            string result = string.Empty;

            SqlTransaction trn = this.sqlConnection.BeginTransaction();
            try
            {
                result = this.repository.DeleteBasis(jobNo, trn, userInfo);
                trn.Commit();
            }
            catch (Exception)
            {
                trn.Rollback();
                throw;
            }

            return result;
        }

        /// <summary>
        /// StoredProcedureTest
        /// </summary>
        /// <remarks>
        /// 一括更新する
        /// ストアドプロシージャでSYS_UPD_DATEを一括更新する
        /// </remarks>
        /// <param name="jobNo">ジョブNO</param>
        /// <param name="userInfo">ユーザー情報</param>
        /// <returns>件数</returns>
        public string StoredProcedureTest(string jobNo, UserInfo userInfo)
        {
            string result = string.Empty;

            SqlTransaction trn = this.sqlConnection.BeginTransaction();
            try
            {
                result = this.repository.StoredProcedureTest(jobNo, trn, userInfo);
                trn.Commit();
            }
            catch (Exception)
            {
                trn.Rollback();
                throw;
            }

            return result;
        }
    }
}
