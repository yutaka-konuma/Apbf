// <copyright file="AbpfDataReaderEnumerable.cs" company="Above Beyond Project Framework">
// Copyright (c) Above Beyond Project Framework. All rights reserved.
// </copyright>

/// <summary>
/// Abpf基盤リポジトリ
/// </summary>
namespace AbpfBase.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using AbpfUtil.Models;
    using AbpfUtil.Util;
    using Microsoft.Data.SqlClient;

    /// <summary>
    /// Modelで返すDataReader+ログ出力
    /// </summary>
    /// <typeparam name="TSource">Model</typeparam>
    public class AbpfDataReaderEnumerable<TSource>
    {
        // プロパティ
        private readonly List<PropertyInfo> listProp = new List<PropertyInfo>();
        private readonly List<string> listPropName = new List<string>();

        // SQLCommand
        private readonly SqlCommand com;

        /// <summary>
        /// Initializes a new instance of the <see cref="AbpfDataReaderEnumerable{TSource}"/> class.
        /// </summary>
        /// <param name="com">SQLCommand</param>
        /// <param name="userInfo">ユーザー情報</param>
        /// <param name="memberName">メンバー名（呼び出し側は指定しない）</param>
        /// <param name="filePath">ファイル名（呼び出し側は指定しない）</param>
        /// <param name="lineNumber">行番号（呼び出し側は指定しない）</param>
        public AbpfDataReaderEnumerable(
            SqlCommand com,
            UserInfo userInfo,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = -1)
        {
            // SQL実行ログをここで出力
            AbpfLogUtil.LogDebug(com, userInfo, memberName, filePath, lineNumber);

            this.com = com;
            foreach (var prop in typeof(TSource).GetProperties())
            {
                this.listProp.Add(prop);
                this.listPropName.Add(AbpfCommonUtil.GetSnakeCase(prop.Name));
            }
        }

        /// <summary>
        /// データ変換
        /// </summary>
        /// <returns>modelでの検索結果を返す</returns>
        public IEnumerable<TSource> AsEnumerable()
        {
            using (var sdr = this.com.ExecuteReader())
            {
                while (sdr.Read())
                {
                    TSource cls = Activator.CreateInstance<TSource>();
                    for (var i = 0; i < this.listProp.Count; i++)
                    {
                        try
                        {
                            // Modelと検索結果で一致しない項目はスルー
                            for (var col = 0; col < sdr.FieldCount; col++)
                            {
                                if (this.listPropName[i].ToUpper() == sdr.GetName(col).ToUpper())
                                {
                                    this.listProp[i].SetValue(cls, sdr[col]);
                                }
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }

                    yield return cls;
                }
            }
        }
    }
}
