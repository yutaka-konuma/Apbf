// <copyright file="GoodsMasterAPITransactionService.cs" company="Above Beyond Project Framework">
// Copyright (c) Above Beyond Project Framework. All rights reserved.
// </copyright>

namespace Sample02GoodsMaster.Service
{
    using System;
    using System.Collections.Generic;
    using AbpfUtil.Models;
    using Microsoft.Data.SqlClient;
    using Microsoft.Extensions.Logging;
    using Sample01Base.Service;
    using Sample02GoodsMaster.Models;
    using Sample02GoodsMaster.Repository;

    /// <summary>
    /// 商品マスタAPIサービス
    /// </summary>
    public class GoodsMasterAPIService : SampleServiceBase
    {
        // 標準ロガー
        private ILoggerFactory logger;

        // 商品マスタAPIレポジトリ
        private GoodsMasterAPIRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="GoodsMasterAPIService"/> class.
        /// </summary>
        /// <param name="logger">標準ロガー</param>
        public GoodsMasterAPIService(ILoggerFactory logger)
            : base(logger)
        {
            this.logger = logger;
            this.repository = new GoodsMasterAPIRepository(this.sqlConnection);
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
            return this.repository.Get(goodsCd, userInfo);
        }

        /// <summary>
        /// Post(Insert)
        /// </summary>
        /// <remarks>
        /// 新規登録する
        /// トランザクション管理する
        /// </remarks>
        /// <param name="form">商品マスタデータ</param>
        /// <param name="userInfo">ユーザー情報</param>
        /// <returns>処理件数</returns>
        public string Post(GoodsMasterData form, UserInfo userInfo)
        {
            string result = string.Empty;

            SqlTransaction trn = this.sqlConnection.BeginTransaction();
            try
            {
                result = this.repository.Post(form, trn, userInfo);
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
        /// Put(Update)
        /// </summary>
        /// <remarks>
        /// 更新する
        /// </remarks>
        /// <param name="form">商品マスタデータ</param>
        /// <param name="userInfo">ユーザー情報</param>
        /// <returns>処理件数</returns>
        public string Put(GoodsMasterData form, UserInfo userInfo)
        {
            string result = string.Empty;

            SqlTransaction trn = this.sqlConnection.BeginTransaction();
            try
            {
                result = this.repository.Put(form, trn, userInfo);
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
        /// Delete
        /// </summary>
        /// <remarks>
        /// 削除する
        /// </remarks>
        /// <param name="goodsCd">商品コード</param>
        /// <param name="userInfo">ユーザー情報</param>
        /// <returns>件数</returns>
        public string Delete(string goodsCd, UserInfo userInfo)
        {
            string result = string.Empty;

            SqlTransaction trn = this.sqlConnection.BeginTransaction();
            try
            {
                result = this.repository.Delete(goodsCd, trn, userInfo);
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
