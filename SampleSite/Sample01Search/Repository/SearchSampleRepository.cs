// <copyright file="SearchSampleRepository.cs" company="Above Beyond Project Framework">
// Copyright (c) Above Beyond Project Framework. All rights reserved.
// </copyright>

namespace Search01Search.Repository
{
    using AbpfBase.Repository;
    using AbpfUtil.Models;
    using AbpfUtil.Util;
    using Microsoft.Data.SqlClient;
    using Sample01Base.Repository;
    using Search01Search.Models;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// 検索サンプルリポジトリ
    /// </summary>
    public class SearchSampleRepository : SampleRepositoryBase
    {
        // SQLConnection
        private readonly SqlConnection con;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchSampleRepository"/> class.
        /// </summary>
        /// <param name="con">DB接続</param>
        public SearchSampleRepository(SqlConnection con)
            : base(con)
        {
            this.con = con;
        }

        /// <summary>
        /// 検索実行
        /// </summary>
        /// <param name="postCd">検索文字列</param>
        /// <param name="userInfo">ユーザー情報</param>
        /// <returns>検索結果リスト</returns>
        public List<SearchSampleViewData> Search(string postCd, UserInfo userInfo)
        {
            List<SearchSampleViewData> results = new List<SearchSampleViewData>();
            List<SqlParameter> paramList = new List<SqlParameter>();

            StringBuilder sqlstr = new StringBuilder(@$"
SELECT * FROM MPOST
            ");

            if (string.IsNullOrEmpty(postCd) == false && postCd.Trim().Length > 0)
            {
                sqlstr.AppendLine(@$"
WHERE POSTCD LIKE @POSTCD
                ");
                paramList.Add(AbpfDBUtil.GetSqlParameter("POSTCD", postCd + '%'));
            }

            using (SqlCommand com = new SqlCommand(sqlstr.ToString(), this.con))
            {
                if (paramList.Count > 0)
                {
                    com.Parameters.AddRange(paramList.ToArray());
                }

                var dre = new AbpfDataReaderEnumerable<SearchSampleViewData>(com, userInfo);
                foreach (SearchSampleViewData row in dre.AsEnumerable())
                {
                    results.Add(row);
                }
            }

            return results;
        }
    }
}
