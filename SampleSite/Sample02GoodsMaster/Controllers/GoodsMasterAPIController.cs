// <copyright file="GoodsMasterAPIController.cs" company="Above Beyond Project Framework">
// Copyright (c) Above Beyond Project Framework. All rights reserved.
// </copyright>

namespace Sample02GoodsMaster.Controllers
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Sample01Base.Controllers;
    using Sample02GoodsMaster.Models;
    using Sample02GoodsMaster.Service;

    /// <summary>
    /// 商品マスタAPIコントローラー
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class GoodsMasterAPIController : SampleAPIControllerBase
    {
        private ILoggerFactory logger;

        // 商品マスタAPIトランザクションサービス
        private GoodsMasterAPIService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="GoodsMasterAPIController"/> class.
        /// 商品マスタAPIコントローラー
        /// </summary>
        /// <param name="logger">標準ロガー</param>
        public GoodsMasterAPIController(ILoggerFactory logger)
            : base(logger)
        {
            this.logger = logger;

            this.service = new GoodsMasterAPIService(this.logger);
        }

        /// <summary>
        /// 商品マスタ取得
        /// Get: api/GoodsMasterAPI
        /// </summary>
        /// <returns>全データを返す</returns>
        [HttpGet]
        public IEnumerable<GoodsMasterData> Get()
        {
            // サービス実行
            var result = this.service.Get(string.Empty, this.GetUserInfo(this.HttpContext));

            return result;
        }

        /// <summary>
        /// 商品マスタ検索
        /// Get: api/GoodsMasterAPI/商品コード
        /// </summary>
        /// <param name="goodsCd">商品コード</param>
        /// <returns>検索結果データを返す</returns>
        [HttpGet("{goodsCd}")]
        public IEnumerable<GoodsMasterData> Get(string goodsCd)
        {
            // サービス実行
            var result = this.service.Get(goodsCd, this.GetUserInfo(this.HttpContext));

            return result;
        }

        /// <summary>
        /// 商品マスタ新規登録
        /// Post: api/GoodsMasterAPI
        /// </summary>
        /// <param name="form">入力フォーム</param>
        /// <returns>処理件数</returns>
        [HttpPost]
        public string Post(GoodsMasterData form)
        {
            // サービス実行
            var result = this.service.Post(form, this.GetUserInfo(this.HttpContext));

            return result;
        }

        /// <summary>
        /// 商品マスタ更新
        /// Put: api/GoodsMasterAPI
        /// </summary>
        /// <param name="form">入力フォーム</param>
        /// <returns>処理件数</returns>
        [HttpPut]
        public string Put(GoodsMasterData form)
        {
            // サービス実行
            var result = this.service.Put(form, this.GetUserInfo(this.HttpContext));

            return result;
        }

        /// <summary>
        /// 商品マスタ削除
        /// Delete: api/GoodsMasterAPI/商品コード
        /// </summary>
        /// <param name="goodsCd">商品コード</param>
        /// <returns>検索結果データを返す</returns>
        [HttpDelete("{goodsCd}")]
        public string Delete(string goodsCd)
        {
            // サービス実行
            var result = this.service.Delete(goodsCd, this.GetUserInfo(this.HttpContext));

            return result;
        }
    }
}
