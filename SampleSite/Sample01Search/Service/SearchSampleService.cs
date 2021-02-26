// <copyright file="SearchSampleService.cs" company="Above Beyond Project Framework">
// Copyright (c) Above Beyond Project Framework. All rights reserved.
// </copyright>

namespace Search01Search.Service
{
    using System;
    using AbpfUtil.Models;
    using AbpfUtil.Util;
    using Microsoft.Extensions.Logging;
    using Sample01Base.Service;
    using Search01Search.Models;
    using Search01Search.Repository;

    /// <summary>
    /// 検索サンプルサービス
    /// </summary>
    public class SearchSampleService : SampleServiceBase
    {
        // 標準ロガー
        private readonly ILoggerFactory logger;

        // 検索サンプルレポジトリ
        private SearchSampleRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchSampleService"/> class.
        /// </summary>
        /// <param name="logger">標準ロガー</param>
        public SearchSampleService(ILoggerFactory logger, bool readOnlyFlg = false)
            : base(logger, readOnlyFlg)
        {
            try
            {
                this.logger = logger;
                this.repository = new SearchSampleRepository(this.sqlConnection);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        /// <summary>
        /// 検索実行
        /// </summary>
        /// <param name="nameStr">検索文字列</param>
        /// <param name="userInfo">ユーザー情報</param>
        /// <returns>検索結果リスト</returns>
        public SearchSampleSearchResult Search(string nameStr, UserInfo userInfo)
        {
            AbpfLogUtil.LogInfo("検索", userInfo);

            SearchSampleSearchResult ret = new SearchSampleSearchResult();
            try
            {
                // サービス実行
                ret.searchSampleViewDataList = this.repository.Search(nameStr, userInfo);
                ret.HasError = false;
            }
            catch (System.Exception e)
            {
                ret.HasError = true;
                ret.ErrorMessage = e.ToString();
            }

            return ret;
        }
    }
}
