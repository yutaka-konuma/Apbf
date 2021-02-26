// <copyright file="BasisAPIController.cs" company="Above Beyond Project Framework">
// Copyright (c) Above Beyond Project Framework. All rights reserved.
// </copyright>

namespace Sample03Basis.Controllers
{
    using System.Collections.Generic;
    using Sample03Basis.Service;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Sample01Base.Controllers;
    using Sample03Basis.Models;

    /// <summary>
    /// 基礎テーブルAPIコントローラー
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BasisAPIController : SampleAPIControllerBase
    {
        private ILoggerFactory logger;

        // 基礎テーブルAPIトランザクションサービス
        private BasisAPIService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="BasisAPIController"/> class.
        /// 基礎テーブルAPIコントローラー
        /// </summary>
        /// <param name="logger">標準ロガー</param>
        public BasisAPIController(ILoggerFactory logger)
            : base(logger)
        {
            this.logger = logger;
            this.service = new BasisAPIService(this.logger);
        }

        /// <summary>
        /// 基礎テーブル取得
        /// Get: api/BasisAPI
        /// </summary>
        /// <returns>全データを返す</returns>
        [HttpGet]
        public IEnumerable<BasisTableData> Get()
        {
            var result = this.service.GetBasis(string.Empty, this.GetUserInfo(this.HttpContext));

            return result;
        }

        /// <summary>
        /// 基礎テーブル検索
        /// Get: api/BasisAPI/ジョブNO
        /// </summary>
        /// <param name="jobNo">ジョブNO</param>
        /// <returns>検索結果データを返す</returns>
        [HttpGet("{jobNo}")]
        public IEnumerable<BasisTableData> Get(string jobNo)
        {
            var result = this.service.GetBasis(jobNo, this.GetUserInfo(this.HttpContext));

            return result;
        }

        /// <summary>
        /// 基礎テーブル検索
        /// Get: api/BasisAPI/ジョブNO
        /// </summary>
        /// <param name="jobNo">ジョブNO</param>
        /// <returns>検索結果データを返す</returns>
        [Route("GetCargo/{jobNo}")]
        [HttpGet]
        public IEnumerable<CargoTableData> GetCargo(string jobNo)
        {
            var result = this.service.GetCargo(jobNo, this.GetUserInfo(this.HttpContext));

            return result;
        }

        /// <summary>
        /// 基礎テーブル新規登録
        /// Post: api/BasisAPI/Insert
        /// </summary>
        /// <param name="form">入力フォーム</param>
        /// <returns>処理件数</returns>
        [Route("Insert")]
        [HttpPost]
        public string InsertBasis(BasisCargoData form)
        {
            var result = this.service.InsertBasis(form, this.GetUserInfo(this.HttpContext));

            return result;
        }

        /// <summary>
        /// 基礎テーブル更新
        /// Post: api/BasisAPI/Update
        /// </summary>
        /// <param name="form">入力フォーム</param>
        /// <returns>処理件数</returns>
        [Route("Update")]
        [HttpPost]
        public string UpdateBasis(BasisCargoData form)
        {
            var result = this.service.UpdateBasis(form, this.GetUserInfo(this.HttpContext));

            return result;
        }

        /// <summary>
        /// 基礎テーブル削除
        /// Delete: api/BasisAPI/Delete/ジョブNO
        /// </summary>
        /// <param name="jobNo">ジョブNO</param>
        /// <returns>検索結果データを返す</returns>
        [Route("Delete/{jobNo}")]
        [HttpPost]
        public string DeleteBasis(string jobNo)
        {
            var result = this.service.DeleteBasis(jobNo, this.GetUserInfo(this.HttpContext));

            return result;
        }

        /// <summary>
        /// 一括更新テスト
        /// StoredProcedure: api/StoredProcedureTest/ジョブNO
        /// </summary>
        /// <param name="jobNo">ジョブNO</param>
        /// <returns>検索結果データを返す</returns>
        [Route("StoredProcedureTest/{jobNo}")]
        [HttpPost]
        public string StoredProcedureTest(string jobNo)
        {
            var result = this.service.StoredProcedureTest(jobNo, this.GetUserInfo(this.HttpContext));

            return result;
        }
    }
}
