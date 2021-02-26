// <copyright file="ImpEmpSampleService.cs" company="Above Beyond Project Framework">
// Copyright (c) Above Beyond Project Framework. All rights reserved.
// </copyright>

namespace Sample07ImpExp.Service
{
    using System;
    using System.IO;
    using AbpfUtil.Models;
    using AbpfUtil.Util;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Data.SqlClient;
    using Microsoft.Extensions.Logging;
    using Sample01Base.Service;
    using Search01Search.Repository;

    public class ImpExpSampleService : SampleServiceBase
    {
        private ILoggerFactory loggerFactory;

        // コンテナ名称
        private const string containerName = "lin-abpf-prot-hdd-temp";

        // 検索サンプルレポジトリ
        private ImpExpSampleRepository impExpSampleRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImpExpSampleService"/> class.
        /// </summary>
        /// <param name="logger">logger.</param>
        /// <param name="userInfo">userInfo.</param>
        public ImpExpSampleService(ILoggerFactory logger)
            : base(logger)
        {
            try
            {
                this.loggerFactory = logger;
                this.impExpSampleRepository = new ImpExpSampleRepository(this.sqlConnection);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public Stream ExpCsv(string fileName, UserInfo userInfo)
        {
            Stream ret;
            try
            {
                // サービス実行
                ret = this.impExpSampleRepository.ExpCsv(fileName, userInfo);
            }
            catch (System.Exception e)
            {
                throw;
            }

            // 指定コンテナのファイルを取得（ストリーム形式）
            return ret;
        }

        public Stream ExpXlsxDioDocs(string fileName, UserInfo userInfo)
        {
            Stream ret;
            try
            {
                // サービス実行
                ret = this.impExpSampleRepository.ExpXlsxDioDocs(fileName, userInfo);
            }
            catch (System.Exception e)
            {
                throw;
            }

            // 指定コンテナのファイルを取得（ストリーム形式）
            return ret;
        }

        public Stream ExpXlsxEPPlus(string fileName, UserInfo userInfo)
        {
            Stream ret;
            try
            {
                // サービス実行
                ret = this.impExpSampleRepository.ExpXlsxEPPlus(fileName, userInfo);
            }
            catch (System.Exception e)
            {
                throw;
            }

            // 指定コンテナのファイルを取得（ストリーム形式）
            return ret;
        }

        public int ImpCsv(IFormFile uploadFile, UserInfo userInfo)
        {
            int result = 0;
            SqlTransaction trn = this.sqlConnection.BeginTransaction();
            try
            {
                // 
                result = this.impExpSampleRepository.ImpCsv(uploadFile, trn, userInfo);
                trn.Commit();
            }
            catch (Exception)
            {
                trn.Rollback();
                throw;
            }
            return result;
        }

        public int ImpXlsxDioDocs(IFormFile uploadFile, UserInfo userInfo)
        {
            int result = 0;
            SqlTransaction trn = this.sqlConnection.BeginTransaction();
            try
            {
                // 
                result = this.impExpSampleRepository.ImpXlsxDioDocs(uploadFile, trn, userInfo);
                if (result > 0)
                {
                    trn.Commit();
                }
                else
                {
                    trn.Rollback();
                }
            }
            catch (Exception)
            {
                trn.Rollback();
                throw;
            }
            return result;
        }

        public int ImpXlsxEPPlus(IFormFile uploadFile, UserInfo userInfo)
        {
            int result = 0;
            SqlTransaction trn = this.sqlConnection.BeginTransaction();
            try
            {
                // 
                result = this.impExpSampleRepository.ImpXlsxEPPlus(uploadFile, trn, userInfo);
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
