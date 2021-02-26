// <copyright file="AbpfStringUtil.cs" company="Above Beyond Project Framework">
// Copyright (c) Above Beyond Project Framework. All rights reserved.
// </copyright>

namespace AbpfUtil.Util
{
    using System.Text;

    /// <summary>
    /// 文字列操作処理
    /// </summary>
    public static class AbpfStringUtil
    {
        /// <summary>
        /// ShifJISのバイト数取得
        /// </summary>
        /// <param name="str">対象文字列</param>
        /// <returns>バイト数</returns>
        public static int LengthB(
            string str)
        {
            Encoding sjisEnc = Encoding.GetEncoding("Shift_JIS");
            return sjisEnc.GetByteCount(str);
        }

        /// <summary>
        /// 文字列の未入力判定
        /// </summary>
        /// <param name="str">対象文字列</param>
        /// <remarks>空白のみなら未入力としてチェックする</remarks>
        /// <returns>未入力ならtrue</returns>
        public static bool IsNoInput(
            string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }
    }
}
