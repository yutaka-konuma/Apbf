// <copyright file="AbpfLogUtil.cs" company="Above Beyond Project Framework">
// Copyright (c) Above Beyond Project Framework. All rights reserved.
// </copyright>

namespace AbpfUtil.Util
{
    using System;
    using System.Runtime.CompilerServices;
    using AbpfUtil.Models;
    using Microsoft.Data.SqlClient;
    using NLog;

    /// <summary>
    /// ログ出力共通機能
    /// </summary>
    public static class AbpfLogUtil
    {
        /// <summary>
        /// ログ出力（Information)
        /// </summary>
        /// <remarks>
        /// 呼び出しクラスを表示するログ
        /// </remarks>
        /// <param name="message">ログメッセージ</param>
        /// <param name="userInfo">ユーザー情報</param>
        /// <param name="memberName">メンバー名（呼び出し側は指定しない）</param>
        /// <param name="filePath">ファイル名（呼び出し側は指定しない）</param>
        /// <param name="lineNumber">行番号（呼び出し側は指定しない）</param>
        public static void LogInfo(
            string message,
            UserInfo userInfo,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = -1)
        {
            string uId = GetUId(userInfo);

            string className = System.IO.Path.GetFileNameWithoutExtension(filePath);
            string msg = string.Format("{0}\t{1}\t{2}\t{3}\t{4}", Environment.MachineName, memberName, className, uId, CheckMessage(message));

            // ユーザーIDをつけたファイル名で出力
            LogBase(Microsoft.Extensions.Logging.LogLevel.Information, uId, msg);
        }

        /// <summary>
        /// ログ出力（Debug)
        /// </summary>
        /// <remarks>
        /// 呼び出しクラスを表示するログ
        /// </remarks>
        /// <param name="message">ログメッセージ</param>
        /// <param name="userInfo">ユーザー情報</param>
        /// <param name="memberName">メンバー名（呼び出し側は指定しない）</param>
        /// <param name="filePath">ファイル名（呼び出し側は指定しない）</param>
        /// <param name="lineNumber">行番号（呼び出し側は指定しない）</param>
        public static void LogDebug(
            string message,
            UserInfo userInfo,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = -1)
        {
            string uId = GetUId(userInfo);
            string className = System.IO.Path.GetFileNameWithoutExtension(filePath);
            string msg = string.Format("{0}\t{1}\t{2}\t{3}\t{4}", Environment.MachineName, memberName, className, uId, CheckMessage(message));

            // ユーザーIDをつけたファイル名で出力
            LogBase(Microsoft.Extensions.Logging.LogLevel.Debug, uId, msg);
        }

        /// <summary>
        /// ログ出力（Warning)
        /// </summary>
        /// <remarks>
        /// 呼び出しクラスを表示するログ
        /// </remarks>
        /// <param name="message">ログメッセージ</param>
        /// <param name="userInfo">ユーザー情報</param>
        /// <param name="memberName">メンバー名（呼び出し側は指定しない）</param>
        /// <param name="filePath">ファイル名（呼び出し側は指定しない）</param>
        /// <param name="lineNumber">行番号（呼び出し側は指定しない）</param>
        public static void LogWarning(
            string message,
            UserInfo userInfo,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = -1)
        {
            string uId = GetUId(userInfo);
            string className = System.IO.Path.GetFileNameWithoutExtension(filePath);
            string msg = string.Format("{0}\t{1}\t{2}\t{3}\t{4}", Environment.MachineName, memberName, className, uId, CheckMessage(message));

            // ユーザーIDをつけたファイル名で出力
            LogBase(Microsoft.Extensions.Logging.LogLevel.Warning, uId, msg);
        }

        /// <summary>
        /// ログ出力（Error)
        /// </summary>
        /// <remarks>
        /// 呼び出しクラスを表示するログ
        /// </remarks>
        /// <param name="message">ログメッセージ</param>
        /// <param name="userInfo">ユーザー情報</param>
        /// <param name="memberName">メンバー名（呼び出し側は指定しない）</param>
        /// <param name="filePath">ファイル名（呼び出し側は指定しない）</param>
        /// <param name="lineNumber">行番号（呼び出し側は指定しない）</param>
        public static void LogError(
            string message,
            UserInfo userInfo,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = -1)
        {
            string uId = GetUId(userInfo);
            string className = System.IO.Path.GetFileNameWithoutExtension(filePath);
            string msg = string.Format("{0}\t{1}\t{2}\t{3}\t{4}", Environment.MachineName, memberName, className, uId, CheckMessage(message));

            // ユーザーIDをつけたファイル名で出力
            LogBase(Microsoft.Extensions.Logging.LogLevel.Error, uId, msg);
        }

        /// <summary>
        /// ログ出力（Information)
        /// </summary>
        /// <remarks>
        /// クラスを指定するログ
        /// </remarks>
        /// <param name="functionID">機能名</param>
        /// <param name="className">クラス名</param>
        /// <param name="message">ログメッセージ</param>
        /// <param name="userInfo">ユーザー情報</param>
        /// <param name="dt">ログ日時</param>
        public static void LogInfoBase(string functionID, string className, string message, UserInfo userInfo, DateTime dt)
        {
            string uId = GetUId(userInfo);
            string msg = string.Format("{0}\t{1}\t{2}\t{3}\t{4}", Environment.MachineName, functionID, className, uId, CheckMessage(message));

            // ユーザーIDをつけたファイル名で出力
            LogBase(Microsoft.Extensions.Logging.LogLevel.Information, uId, msg, dt);
        }

        /// <summary>
        /// ログ出力（Debug)
        /// </summary>
        /// <remarks>
        /// クラスを指定するログ
        /// </remarks>
        /// <param name="functionID">機能名</param>
        /// <param name="className">クラス名</param>
        /// <param name="message">ログメッセージ</param>
        /// <param name="userInfo">ユーザー情報</param>
        /// <param name="dt">ログ日時</param>
        public static void LogDebugBase(string functionID, string className, string message, UserInfo userInfo, DateTime dt)
        {
            string uId = GetUId(userInfo);
            string msg = string.Format("{0}\t{1}\t{2}\t{3}\t{4}", Environment.MachineName, functionID, className, uId, CheckMessage(message));

            // ユーザーIDをつけたファイル名で出力
            LogBase(Microsoft.Extensions.Logging.LogLevel.Debug, uId, msg, dt);
        }

        /// <summary>
        /// ログ出力（Warning)
        /// </summary>
        /// <remarks>
        /// クラスを指定するログ
        /// </remarks>
        /// <param name="functionID">機能名</param>
        /// <param name="className">クラス名</param>
        /// <param name="message">ログメッセージ</param>
        /// <param name="userInfo">ユーザー情報</param>
        /// <param name="dt">ログ日時</param>
        public static void LogWarningBase(string functionID, string className, string message, UserInfo userInfo, DateTime dt)
        {
            string uId = GetUId(userInfo);
            string msg = string.Format("{0}\t{1}\t{2}\t{3}\t{4}", Environment.MachineName, functionID, className, uId, CheckMessage(message));

            // ユーザーIDをつけたファイル名で出力
            LogBase(Microsoft.Extensions.Logging.LogLevel.Warning, uId, msg, dt);
        }

        /// <summary>
        /// ログ出力（Error)
        /// </summary>
        /// <remarks>
        /// クラスを指定するログ
        /// </remarks>
        /// <param name="functionID">機能名</param>
        /// <param name="className">クラス名</param>
        /// <param name="message">ログメッセージ</param>
        /// <param name="userInfo">ユーザー情報</param>
        /// <param name="dt">ログ日時</param>
        public static void LogErrorBase(string functionID, string className, string message, UserInfo userInfo, DateTime dt)
        {
            string uId = GetUId(userInfo);
            string msg = string.Format("{0}\t{1}\t{2}\t{3}\t{4}", Environment.MachineName, functionID, className, uId, CheckMessage(message));

            // ユーザーIDをつけたファイル名で出力
            LogBase(Microsoft.Extensions.Logging.LogLevel.Error, uId, msg, dt);
        }

        /// <summary>
        /// SQLログ出力（Information)
        /// </summary>
        /// <remarks>
        /// 呼び出しクラスを表示するSQLログ
        /// </remarks>
        /// <param name="sql">SqlCommand</param>
        /// <param name="userInfo">ユーザー情報</param>
        /// <param name="memberName">メンバー名（呼び出し側は指定しない）</param>
        /// <param name="filePath">ファイル名（呼び出し側は指定しない）</param>
        /// <param name="lineNumber">行番号（呼び出し側は指定しない）</param>
        public static void LogInfo(
            SqlCommand sql,
            UserInfo userInfo,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = -1)
        {
            string uId = GetUId(userInfo);
            string className = System.IO.Path.GetFileNameWithoutExtension(filePath);

            // 改行を空白に変換
            string command = sql.CommandText.Replace("\r\n", " ").Trim();

            // SQL文のパラメータを置換する
            for (var i = 0; i < sql.Parameters.Count; i++)
            {
                // 文字型は引用符をつける
                if (sql.Parameters[i].DbType == System.Data.DbType.String)
                {
                    command = command.Replace("@" + sql.Parameters[i].ParameterName.Trim(), "'" + sql.Parameters[i].SqlValue.ToString().Trim() + "'");
                }
                else
                {
                    command = command.Replace("@" + sql.Parameters[i].ParameterName.Trim(), sql.Parameters[i].SqlValue.ToString().Trim());
                }
            }

            string message = string.Format("{0}", command);

            string msg = string.Format("{0}\t{1}\t{2}\t{3}\t{4}", Environment.MachineName, memberName, className, uId, CheckMessage(message));

            // ユーザーIDに"SQL"をつけたファイル名で出力
            LogBase(Microsoft.Extensions.Logging.LogLevel.Information, "SQL" + uId, msg);
        }

        /// <summary>
        /// SQLログ出力（Debug)
        /// </summary>
        /// <remarks>
        /// 呼び出しクラスを表示するSQLログ
        /// </remarks>
        /// <param name="sql">SqlCommand</param>
        /// <param name="userInfo">ユーザー情報</param>
        /// <param name="memberName">メンバー名（呼び出し側は指定しない）</param>
        /// <param name="filePath">ファイル名（呼び出し側は指定しない）</param>
        /// <param name="lineNumber">行番号（呼び出し側は指定しない）</param>
        public static void LogDebug(
            SqlCommand sql,
            UserInfo userInfo,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = -1)
        {
            string uId = GetUId(userInfo);
            string className = System.IO.Path.GetFileNameWithoutExtension(filePath);

            // 改行を空白に変換
            string command = sql.CommandText.Replace("\r\n", " ").Trim();

            // SQL文のパラメータを置換する
            for (var i = 0; i < sql.Parameters.Count; i++)
            {
                if (sql.Parameters[i].SqlValue != null)
                {
                    // 文字型は引用符をつける
                    if (sql.Parameters[i].DbType == System.Data.DbType.String)
                    {
                        command = command.Replace("@" + sql.Parameters[i].ParameterName.Trim(), "'" + sql.Parameters[i].SqlValue.ToString().Trim() + "'");
                    }
                    else
                    {
                        command = command.Replace("@" + sql.Parameters[i].ParameterName.Trim(), sql.Parameters[i].SqlValue.ToString().Trim());
                    }
                }
                else
                {
                    command = command.Replace("@" + sql.Parameters[i].ParameterName.Trim(), "NULL");
                }
            }

            string message = string.Format("{0}", command);

            string msg = string.Format("{0}\t{1}\t{2}\t{3}\t{4}", Environment.MachineName, memberName, className, uId, CheckMessage(message));

            // ユーザーIDに"SQL"をつけたファイル名で出力
            LogBase(Microsoft.Extensions.Logging.LogLevel.Debug, "SQL" + uId, msg);
        }

        /// <summary>
        /// SQLログ出力（Warning)
        /// </summary>
        /// <remarks>
        /// 呼び出しクラスを表示するSQLログ
        /// </remarks>
        /// <param name="sql">SqlCommand</param>
        /// <param name="userInfo">ユーザー情報</param>
        /// <param name="memberName">メンバー名（呼び出し側は指定しない）</param>
        /// <param name="filePath">ファイル名（呼び出し側は指定しない）</param>
        /// <param name="lineNumber">行番号（呼び出し側は指定しない）</param>
        public static void LogWarning(
            SqlCommand sql,
            UserInfo userInfo,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = -1)
        {
            string uId = GetUId(userInfo);
            string className = System.IO.Path.GetFileNameWithoutExtension(filePath);

            // 改行を空白に変換
            string command = sql.CommandText.Replace("\r\n", " ").Trim();

            // SQL文のパラメータを置換する
            for (var i = 0; i < sql.Parameters.Count; i++)
            {
                // 文字型は引用符をつける
                if (sql.Parameters[i].DbType == System.Data.DbType.String)
                {
                    command = command.Replace("@" + sql.Parameters[i].ParameterName.Trim(), "'" + sql.Parameters[i].SqlValue.ToString().Trim() + "'");
                }
                else
                {
                    command = command.Replace("@" + sql.Parameters[i].ParameterName.Trim(), sql.Parameters[i].SqlValue.ToString().Trim());
                }
            }

            string message = string.Format("{0}", command);

            string msg = string.Format("{0}\t{1}\t{2}\t{3}\t{4}", Environment.MachineName, memberName, className, uId, CheckMessage(message));

            // ユーザーIDに"SQL"をつけたファイル名で出力
            LogBase(Microsoft.Extensions.Logging.LogLevel.Warning, "SQL" + uId, msg);
        }

        /// <summary>
        /// SQLログ出力（Error)
        /// </summary>
        /// <remarks>
        /// 呼び出しクラスを表示するSQLログ
        /// </remarks>
        /// <param name="sql">SQLCommand</param>
        /// <param name="userInfo">ユーザー情報</param>
        /// <param name="memberName">メンバー名（呼び出し側は指定しない）</param>
        /// <param name="filePath">ファイル名（呼び出し側は指定しない）</param>
        /// <param name="lineNumber">行番号（呼び出し側は指定しない）</param>
        public static void LogError(
            SqlCommand sql,
            UserInfo userInfo,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = -1)
        {
            string uId = GetUId(userInfo);
            string className = System.IO.Path.GetFileNameWithoutExtension(filePath);

            // 改行を空白に変換
            string command = sql.CommandText.Replace("\r\n", " ").Trim();

            // SQL文のパラメータを置換する
            for (var i = 0; i < sql.Parameters.Count; i++)
            {
                // 文字型は引用符をつける
                if (sql.Parameters[i].DbType == System.Data.DbType.String)
                {
                    command = command.Replace("@" + sql.Parameters[i].ParameterName.Trim(), "'" + sql.Parameters[i].SqlValue.ToString().Trim() + "'");
                }
                else
                {
                    command = command.Replace("@" + sql.Parameters[i].ParameterName.Trim(), sql.Parameters[i].SqlValue.ToString().Trim());
                }
            }

            string message = string.Format("{0}", command);

            string msg = string.Format("{0}\t{1}\t{2}\t{3}\t{4}", Environment.MachineName, memberName, className, uId, CheckMessage(message));

            // ユーザーIDに"SQL"をつけたファイル名で出力
            LogBase(Microsoft.Extensions.Logging.LogLevel.Error, "SQL" + uId, msg);
        }

        /// <summary>
        /// 例外処理ログ出力（Error)
        /// </summary>
        /// <remarks>
        /// 呼び出しクラスを表示する例外処理ログ
        /// </remarks>
        /// <param name="ex">Exception</param>
        /// <param name="userInfo">ユーザー情報</param>
        /// <param name="memberName">メンバー名（呼び出し側は指定しない）</param>
        /// <param name="filePath">ファイル名（呼び出し側は指定しない）</param>
        /// <param name="lineNumber">行番号（呼び出し側は指定しない）</param>
        public static void LogError(
            Exception ex,
            UserInfo userInfo,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = -1)
        {
            string className = System.IO.Path.GetFileNameWithoutExtension(filePath);

            string message = string.Format("{0}", ex.Message);

            string uId = GetUId(userInfo);

            // スタックトレースのメッセージは改行削除しない
            string msg = string.Format(
                "{0}\t{1}\t{2}\t{3}\t{4}",
                Environment.MachineName,
                memberName,
                className,
                uId,
                message);

            // ユーザーIDに"ERROR"をつけたファイル名で出力
            LogBase(Microsoft.Extensions.Logging.LogLevel.Error, "ERROR_" + uId, msg);
        }

        /// <summary>
        /// ログ出力基本処理
        /// </summary>
        /// <remarks>
        /// ログレベルなどを指定してログ出力する
        /// 通常NLogで出力、ロガーがある場合にAzureログ出力とする予定
        /// </remarks>
        /// <param name="logLevel">ログレベル</param>
        /// <param name="userID">ユーザーＩＤ</param>
        /// <param name="msg">ログメッセージ</param>
        public static void LogBase(Microsoft.Extensions.Logging.LogLevel logLevel, string userID, string msg)
        {
            LogBase(logLevel, userID, msg, DateTime.Now);
        }

        /// <summary>
        /// ログ出力基本処理
        /// </summary>
        /// <remarks>
        /// ログレベルなどを指定してログ出力する
        /// 通常NLogで出力、ロガーがある場合にAzureログ出力とする予定
        /// </remarks>
        /// <param name="logLevel">ログレベル</param>
        /// <param name="userID">ユーザーＩＤ</param>
        /// <param name="msg">ログメッセージ</param>
        /// <param name="dt">出力日時を指定</param>
        public static void LogBase(Microsoft.Extensions.Logging.LogLevel logLevel, string userID, string msg, DateTime dt)
        {
            // 日時＋ログレベル＋メッセージを渡す
            msg = dt.ToString("yyyy/MM/dd HH:mm:ss.fff") + "\t" + logLevel.ToString() + "\t" + msg + "\t";

            // ↓ Factory、及び、Loggerの生成は、ユーザ毎のファイル名にする為に都度実施している
            var factory = new LogFactory();
            var logger = factory.GetLogger(AbpfConfigUtil.GetValue("NLogEnv", "Rules"));

            // NLog.configの ${var:runtime} を ユーザ識別子 と置換する
            logger.Factory.Configuration.Variables.Add("runtime", userID);

            // ファイル名の変更をFactoryに対して有効化する
            // 注 生成したFactory範囲内の設定が変更されるため
            //    Factoryのインスタンス化を事前に実施する場合、
            //    ログインユーザ単位に生成する必要がある
            factory.ReconfigExistingLoggers();

            // ↑ Factory、及び、Loggerの生成は、ユーザ毎のファイル名にする為に都度実施している

            // ログレベル毎のログ出力を実施
            switch (logLevel)
            {
                case Microsoft.Extensions.Logging.LogLevel.Debug:
                    logger.Debug(msg);
                    break;
                case Microsoft.Extensions.Logging.LogLevel.Information:
                    logger.Info(msg);
                    break;
                case Microsoft.Extensions.Logging.LogLevel.Warning:
                    logger.Warn(msg);
                    break;
                case Microsoft.Extensions.Logging.LogLevel.Error:
                    logger.Error(msg);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// メッセージから不要文字を除外する
        /// </summary>
        /// <remarks>
        /// メッセージ内のタブを半角スペースに変換、改行文字は削除
        /// </remarks>
        /// <param name="message">元メッセージ</param>
        /// <returns>変換後メッセージ</returns>
        private static string CheckMessage(string message)
        {
            // メッセージ内のタブを半角スペースに変換、改行文字は削除
            return message.Replace("\t", " ").Replace("\r", string.Empty).Replace("\n", string.Empty);
        }

        /// <summary>
        /// ログファイル名用のUIdを取得する
        /// </summary>
        /// <remarks>
        /// 返却値の優先度
        ///  1. userInfo 未指定 → NoUser
        ///  2. メールアドレスあり → MailAddress
        ///  3. UId あり → UId
        ///  上記以外 → NoUser
        /// </remarks>
        /// <param name="userInfo">ユーザ情報</param>
        /// <returns>ログファイル名用のUId</returns>
        private static string GetUId(UserInfo userInfo)
        {
            const string NoUser = "NoUser";
            return userInfo == null ? NoUser :
                string.IsNullOrEmpty(userInfo.MailAddress) == false ? userInfo.MailAddress :
                string.IsNullOrEmpty(userInfo.UId) == false ? userInfo.UId :
                NoUser;
        }

    }
}
