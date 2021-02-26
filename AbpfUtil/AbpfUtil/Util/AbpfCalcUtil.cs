// <copyright file="AbpfCalcUtil.cs" company="Above Beyond Project Framework">
// Copyright (c) Above Beyond Project Framework. All rights reserved.
// </copyright>

namespace AbpfUtil.Util
{
    using System;

    /// <summary>
    /// 計算ユーティリティ
    /// </summary>
    public static class AbpfCalcUtil
    {
        /// <summary>
        /// 四捨五入
        /// </summary>
        /// <param name="value">値</param>
        /// <param name="numOfDigits">小数桁数</param>
        /// <returns>結果</returns>
        public static decimal Round(decimal value, int numOfDigits)
        {
            decimal pow = (decimal)Math.Pow(10, numOfDigits);

            return Math.Round(value * pow, MidpointRounding.AwayFromZero) / pow;
        }

        /// <summary>
        /// 切り捨て
        /// </summary>
        /// <param name="value">値</param>
        /// <param name="numOfDigits">小数桁数</param>
        /// <returns>結果</returns>
        public static decimal Trunc(decimal value, int numOfDigits)
        {
            decimal pow = (decimal)Math.Pow(10, numOfDigits);

            return Math.Floor(value * pow) / pow;
        }

        /// <summary>
        /// 切り上げ
        /// </summary>
        /// <param name="value">値</param>
        /// <param name="numOfDigits">小数桁数</param>
        /// <returns>結果</returns>
        public static decimal RoundUp(decimal value, int numOfDigits)
        {
            decimal pow = (decimal)Math.Pow(10, numOfDigits);

            return Math.Ceiling(value * pow) / pow;
        }
    }
}
