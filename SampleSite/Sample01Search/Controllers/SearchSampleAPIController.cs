// <copyright file="SearchSampleAPIController.cs" company="Above Beyond Project Framework">
// Copyright (c) Above Beyond Project Framework. All rights reserved.
// </copyright>

namespace Search01Search.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Sample01Base.Controllers;
    using Search01Search.Models;
    using Search01Search.Service;

    /// <summary>
    /// 検索サンプルAPIコントローラー
    /// </summary>
    [AutoValidateAntiforgeryToken]
    [Route("api/[controller]")]
    [ApiController]
    public class SearchSampleAPIController : SampleAPIControllerBase
    {
        private ILoggerFactory logger;

        // 検索サンプルサービス
        private SearchSampleService searchSampleService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchSampleAPIController"/> class.
        /// </summary>
        /// <param name="logger">標準ロガー</param>
        public SearchSampleAPIController(
            ILoggerFactory logger)
            : base(logger)
        {
            this.logger = logger;
            this.searchSampleService = new SearchSampleService(this.logger, true);
        }

        /// <summary>
        /// GET api/SearchSampleAPI/2701166
        /// </summary>
        /// <param name="postCd">郵便番号</param>
        /// <returns>検索結果</returns>
        [HttpGet("{postCd}")]
        public SearchSampleSearchResult Get(string postCd)
        {
            return this.searchSampleService.Search(postCd, this.GetUserInfo(this.HttpContext));
        }

        /// <summary>
        /// POST: api/SearchSampleAPI
        /// </summary>
        /// <param name="form">フォームデータ</param>
        /// <returns>検索結果</returns>
        [HttpPost]
        public SearchSampleSearchResult Post([FromBody] SearchSampleViewData form)
        {
            return this.searchSampleService.Search(form.Postcd, this.GetUserInfo(this.HttpContext));
        }
    }
}
