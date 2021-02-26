// <copyright file="AbpfCommonUtil.cs" company="Above Beyond Project Framework">
// Copyright (c) Above Beyond Project Framework. All rights reserved.
// </copyright>

namespace AbpfUtil.Util
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using AbpfUtil.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.SignalR;

    /// <summary>
    /// 汎用ユーティリティ
    /// </summary>
    public static class AbpfCommonUtil
    {
        /// <summary>
        /// int値を取得
        /// </summary>
        /// <param name="str">対象文字列</param>
        /// <returns>int</returns>
        public static int GetInt(string str)
        {
            if (int.TryParse(str, out int result))
            {
                return result;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// logn値を取得
        /// </summary>
        /// <param name="str">対象文字列</param>
        /// <returns>long</returns>
        public static long GetLong(string str)
        {
            if (long.TryParse(str, out long result))
            {
                return result;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// float値を取得
        /// </summary>
        /// <param name="str">対象文字列</param>
        /// <returns>long</returns>
        public static float GetFloat(string str)
        {
            if (float.TryParse(str, out float result))
            {
                return result;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// double値を取得
        /// </summary>
        /// <param name="str">対象文字列</param>
        /// <returns>double</returns>
        public static double GetDouble(string str)
        {
            if (double.TryParse(str, out double result))
            {
                return result;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// decimal値を取得
        /// </summary>
        /// <param name="str">対象文字列</param>
        /// <returns>decimal</returns>
        public static decimal GetDecimal(string str)
        {
            if (decimal.TryParse(str, out decimal result))
            {
                return result;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// モデルデータをディクショナリに変換する
        /// </summary>
        /// <typeparam name="T">model</typeparam>
        /// <param name="model">medelデータ</param>
        /// <returns>ディクショナリ</returns>
        public static Dictionary<string, string> ModelToDictionary<T>(this T model)
        {
            var dictionary = model.GetType()
            .GetProperties()
            .Where(t => t.GetValue(model, null) != null)
            .Select(i => (i.Name, i.GetValue(model, null)?.ToString()))
            .ToDictionary(x => x.Name, x => x.Item2);

            return dictionary;
        }

        /// <summary>
        /// キャメルケース（Model）の名前をスネークケース（DB名）に変換
        /// </summary>
        /// <param name="str">キャメルケース（Model）の名前</param>
        /// <returns>スネークケース（DB名）</returns>
        public static string GetSnakeCase(string str)
        {
            var regex = new System.Text.RegularExpressions.Regex("[a-z][A-Z]");
            return regex.Replace(str, s => $"{s.Groups[0].Value[0]}_{s.Groups[0].Value[1]}").ToUpper();
        }

        /// <summary>
        /// スネークケースをアッパーキャメル(パスカル)ケースに変換します
        /// 例) quoted_printable_encode → QuotedPrintableEncode
        /// </summary>
        /// <param name="self">スネークケース（DB名）</param>
        /// <returns>アッパーキャメル(パスカル)ケース</returns>
        public static string GetUpperCamel(this string self)
        {
            if (string.IsNullOrEmpty(self))
            {
                return self;
            }

            return self
                .Split(new[] { '_' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => char.ToUpperInvariant(s[0]) + s.Substring(1, s.Length - 1))
                .Aggregate(string.Empty, (s1, s2) => s1 + s2)
            ;
        }

        /// <summary>
        /// スネークケースをローワーキャメル(キャメル)ケースに変換します
        /// 例) quoted_printable_encode → quotedPrintableEncode
        /// </summary>
        /// <param name="self">スネークケース（DB名）</param>
        /// <returns>ローワーキャメル(キャメル)ケース</returns>
        public static string GetLowerCamel(this string self)
        {
            if (string.IsNullOrEmpty(self))
            {
                return self;
            }

            return self
                .GetUpperCamel()
                .Insert(0, char.ToLowerInvariant(self[0]).ToString()).Remove(1, 1)
            ;
        }

        /// <summary>
        /// ユーザー情報取得
        /// </summary>
        /// <param name="httpContext">Context</param>
        /// <returns>ユーザー情報</returns>
        public static UserInfo GetUserInfo(HttpContext httpContext)
        {
            UserInfo userInfo = new UserInfo();
            if (httpContext == null)
            {
                return userInfo;
            }

            foreach (var claim in httpContext.User.Claims)
            {
                switch (claim.Type)
                {
                    case "name":
                        userInfo.Name = claim.Value;
                        break;
                    case "emails":
                        userInfo.MailAddress = claim.Value;
                        break;
                    case "country":
                        userInfo.Country = claim.Value;
                        break;
                    case "tfp":
                        userInfo.Tfp = claim.Value;
                        break;
                    case "utid":
                        userInfo.UtId = claim.Value;
                        break;
                    case "uid":
                        userInfo.UId = claim.Value;
                        userInfo.UserIdentifier = claim.Value;
                        break;
                    default:
                        break;
                }
            }

            // 画面で選択した言語の設定を取得
            if (httpContext.Connection != null)
            {

                // 隠ぺいメンバの取得
                var headers = httpContext.Request.Headers;
                PropertyInfo acceptLanguage = headers.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Instance).Single(pi => pi.Name == "AcceptLanguage");
                object acceptLanguageValue = acceptLanguage.GetValue(headers, null);

                userInfo.AcceptLanguage = acceptLanguageValue.ToString();

            }

            return userInfo;
        }

        /// <summary>
        /// ユーザー情報取得
        /// </summary>
        /// <param name="httpContext">Context</param>
        /// <returns>ユーザー情報</returns>
        public static UserInfo GetUserInfo(HubCallerContext hubCallerContext)
        {
            UserInfo userInfo = new UserInfo();
            if (hubCallerContext == null)
            {
                return userInfo;
            }

            foreach (var claim in hubCallerContext.User.Claims)
            {
                switch (claim.Type)
                {
                    case "name":
                        userInfo.Name = claim.Value;
                        break;
                    case "emails":
                        userInfo.MailAddress = claim.Value;
                        break;
                    case "country":
                        userInfo.Country = claim.Value;
                        break;
                    case "tfp":
                        userInfo.Tfp = claim.Value;
                        break;
                    case "utid":
                        userInfo.UtId = claim.Value;
                        break;
                    case "uid":
                        userInfo.UId = claim.Value;
                        userInfo.UserIdentifier = claim.Value;
                        break;
                    default:
                        break;
                }
            }

            return userInfo;
        }

    }
}
