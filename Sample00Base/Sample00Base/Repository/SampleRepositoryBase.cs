// <copyright file="SampleRepositoryBase.cs" company="Above Beyond Project Framework">
// Copyright (c) Above Beyond Project Framework. All rights reserved.
// </copyright>

namespace Sample01Base.Repository
{
    using AbpfBase.Repository;
    using Microsoft.Data.SqlClient;

    /// <summary>
    /// サンプルアプリケーション共通リポジトリ
    /// </summary>
    public class SampleRepositoryBase : AbpfRepositoryBase
    {
        // SQLConnection
        private readonly SqlConnection con;

        /// <summary>
        /// Initializes a new instance of the <see cref="SampleRepositoryBase"/> class.
        /// </summary>
        /// <param name="con">DB接続</param>
        public SampleRepositoryBase(SqlConnection con)
            : base(con)
        {
            this.con = con;
        }
    }
}
