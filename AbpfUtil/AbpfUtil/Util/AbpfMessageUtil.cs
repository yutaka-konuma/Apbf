// <copyright file="AbpfMessageUtil.cs" company="Above Beyond Project Framework">
// Copyright (c) Above Beyond Project Framework. All rights reserved.
// </copyright>

namespace AbpfUtil.Util
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Extensions.Localization;

    /// <summary>
    /// メッセージユーティリティ
    /// </summary>
    public static class AbpfMessageUtil
    {
        /// <summary>
        /// メッセージディクショナリ → 将来DB化？
        /// </summary>
        private static Dictionary<string, string> messageDic = new Dictionary<string, string>()
        {
            {
                "E00000", "E00000" // コードに対応するメッセージは、各言語のpoファイルに記述
            },
            {
                "E00001", "E00001"
            },
            {
                "E00002", "E00002"
            },
        };

        /// <summary>
        /// メッセージ取得
        /// </summary>
        /// <param name="id">メッセージID</param>
        /// <param name="opt">パラメータ</param>
        /// <param name="stringLocalizer">ローカライザー</param>
        /// <returns>メッセージ</returns>
        public static string GetMessage(string id, string[] opt, IStringLocalizer stringLocalizer)
        {
            string msg;
            if (messageDic.ContainsKey(id))
            {
                msg = messageDic[id];
            }
            else
            {
                msg = messageDic["E00000"];
            }

            string[] localiOpt = new string[] { };
            foreach (string str in opt)
            {
                Array.Resize(ref localiOpt, localiOpt.Length + 1);
                localiOpt[localiOpt.Length - 1] = stringLocalizer[str];
            }

            return string.Format(msg, localiOpt.ToArray());
        }

        /// <summary>
        /// 全メッセージのローカライズ
        /// </summary>
        /// <param name="stringLocalizer">ローカライザー</param>
        /// <returns>メッセージディクショナリ</returns>
        public static Dictionary<string, string> GetLocalizMessageAll(IStringLocalizer stringLocalizer)
        {
            Dictionary<string, string> localizMessageDic = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> kvp in messageDic)
            {
                localizMessageDic.Add(kvp.Key, stringLocalizer[kvp.Value]);
            }

            return localizMessageDic;
        }
    }
}
