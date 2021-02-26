// <copyright file="AbpfCryptUtil.cs" company="Above Beyond Project Framework">
// Copyright (c) Above Beyond Project Framework. All rights reserved.
// </copyright>

namespace AbpfUtil.Util
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// 暗号化ユーティリティ
    /// </summary>
    public static class AbpfCryptUtil
    {

        private const string EncKey = "1234567890123456";

        /// <summary>
        /// 暗号化
        /// </summary>
        /// <param name="text">対象文字列</param>
        /// <Remark>暗号化キーはappsetting.jsonより取得</Remark>
        /// <returns>暗号化文字列</returns>
        public static string Encrypt(
            string text)
        {
            // 暗号化キーを取得
            byte[] key = Encoding.Unicode.GetBytes(EncKey); // Encoding.Unicode.GetBytes(AbpfConfigUtil.GetValue(AbpfConfigUtil.SectionMail, AbpfConfigUtil.Key));

            // 入力文字列をバイト型配列に変換
            byte[] src = Encoding.Unicode.GetBytes(text);

            // Encryptorを用意する
            using Aes aes = Aes.Create();
            aes.Key = key;
            byte[] iv = aes.IV;

            using (var encryptor = aes.CreateEncryptor(key, iv))

            // 出力ストリームを用意する
            using (var outStream = new MemoryStream())
            {
                // IVを書き出す
                outStream.Write(iv);

                // 暗号化して書き出す
                using (var cs = new CryptoStream(outStream, encryptor, CryptoStreamMode.Write))
                {
                    cs.Write(src, 0, src.Length);
                }

                // Base64文字列に変換して返す
                byte[] result = outStream.ToArray();
                return Convert.ToBase64String(result);
            }
        }

        /// <summary>
        /// 暗号化された文字列を復号化する
        /// </summary>
        /// <param name="sourceString">暗号化された文字列</param>
        /// <Remark>暗号化キーはappsetting.jsonより取得</Remark>
        /// <returns>復号化された文字列</returns>
        public static string Decrypt(
            string sourceString)
        {
            // 暗号化キーを取得
            byte[] key = Encoding.Unicode.GetBytes(EncKey); // Encoding.Unicode.GetBytes(AbpfConfigUtil.GetValue(AbpfConfigUtil.SectionMail, AbpfConfigUtil.Key));

            // /Base64文字列をバイト型配列に変換
            byte[] src = Convert.FromBase64String(sourceString);

            // Decryptorを用意する
            using Aes aes = Aes.Create();
            aes.Key = key;

            // 入力ストリームを開く
            using var inStream = new MemoryStream(src, false);
            byte[] iv = new byte[aes.IV.Length];
            inStream.Read(iv, 0, iv.Length);
            using var decryptor = aes.CreateDecryptor(key, iv);

            // 出力ストリームを用意する
            using (var outStream = new MemoryStream())
            {
                // 復号して一定量ずつ読み出し、それを出力ストリームに書き出す
                using (var cs = new CryptoStream(inStream, decryptor, CryptoStreamMode.Read))
                {
                    byte[] buffer = new byte[4096]; // バッファーサイズはBlockSizeの倍数にする
                    int len = 0;
                    while ((len = cs.Read(buffer, 0, 4096)) > 0)
                    {
                        outStream.Write(buffer, 0, len);
                    }
                }

                // 文字列に変換して返す
                byte[] result = outStream.ToArray();
                return Encoding.Unicode.GetString(result);
            }
        }
    }
}
