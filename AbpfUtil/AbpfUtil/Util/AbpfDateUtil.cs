// <copyright file="AbpfDateUtil.cs" company="Above Beyond Project Framework">
// Copyright (c) Above Beyond Project Framework. All rights reserved.
// </copyright>

namespace AbpfUtil.Util
{
    using System;

    /// <summary>
    /// 日付時刻ユーティリティ
    /// </summary>
    public static class AbpfDateUtil
    {
        /// <summary>
        /// 文字列データ日付変換
        /// </summary>
        /// <remarks>
        /// Varchar(8)の日付文字列データを日付形式に変換
        /// </remarks>
        /// <param name="strYYYYMMDD">Varchar(8)の日付</param>
        /// <returns>YYYY/MM/DD</returns>
        public static string YYYYMMDDToDate(string strYYYYMMDD)
        {
            string strFormat;
            if (strYYYYMMDD.Length == 8)
            {
                strFormat = "yyyyMMdd";
            }
            else if (strYYYYMMDD.Length == 17)
            {
                strFormat = "yyyyMMddHHmmssfff";
            }
            else if (strYYYYMMDD.Length == 9)
            {
                strFormat = "HHmmssfff";
            }
            else
            {
                return string.Empty;
            }

            DateTime dTime;
            if (DateTime.TryParseExact(strYYYYMMDD, strFormat, null, System.Globalization.DateTimeStyles.None, out dTime))
            {
                return string.Format("yyyy/MM/dd", dTime);
            }

            return string.Empty;
        }

        /// <summary>
        /// 日付データ文字列変換
        /// </summary>
        /// <remarks>
        /// 日付形式をVarchar(8)の日付文字列データに変換
        /// </remarks>
        /// <param name="strDate">YYYY/MM/DD</param>
        /// <returns>Varchar(8)の日付</returns>
        public static string DateToYYYYMMDD(string strDate)
        {
            DateTime dTime = DateTime.Now;
            if (DateTime.TryParseExact(strDate, "yyyy/mm/dd", null, System.Globalization.DateTimeStyles.None, out dTime))
            {
                return string.Format("yyyyMMdd", dTime);
            }

            return string.Empty;
        }

        /// <summary>
        /// 文字列データ時刻変換
        /// </summary>
        /// <remarks>
        /// Varchar(9)の時刻文字列データを時刻形式に変換
        /// </remarks>
        /// <param name="strHHMMSSFFF">Varchar(9)の時刻</param>
        /// <returns>HH:mm:ss.fff</returns>
        public static string HHMMSSFFFToTime(string strHHMMSSFFF)
        {
            string strFormat;
            if (strHHMMSSFFF.Length == 8)
            {
                strFormat = "yyyyMMdd";
            }
            else if (strHHMMSSFFF.Length == 17)
            {
                strFormat = "yyyyMMddHHmmssfff";
            }
            else if (strHHMMSSFFF.Length == 9)
            {
                strFormat = "HHmmssfff";
            }
            else
            {
                return string.Empty;
            }

            DateTime dTime;
            if (DateTime.TryParseExact(strHHMMSSFFF, strFormat, null, System.Globalization.DateTimeStyles.None, out dTime))
            {
                return string.Format("HH:mm:ss.fff", dTime);
            }

            return string.Empty;
        }

        /// <summary>
        /// 文字列データ時刻変換
        /// </summary>
        /// <remarks>
        /// Varchar(9)の時刻文字列データを時刻形式に変換
        /// </remarks>
        /// <param name="strTrme">HH:mm:ss.fff</param>
        /// <returns>Varchar(9)の時刻</returns>
        public static string TimeToHHMMSSFFF(string strTrme)
        {
            DateTime dTime;
            if (DateTime.TryParseExact(strTrme, "HH:mm:ss.fff", null, System.Globalization.DateTimeStyles.None, out dTime))
            {
                return string.Format("HHmmssfff", dTime);
            }

            return string.Empty;
        }
    }
}
