// <copyright file="AbpfMailUtil.cs" company="Above Beyond Project Framework">
// Copyright (c) Above Beyond Project Framework. All rights reserved.
// </copyright>

namespace AbpfUtil.Util
{
    using System;
    using System.Net;
    using System.Net.Mail;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;
    using AbpfUtil.Models;
    using Microsoft.Exchange.WebServices.Data;
    using Microsoft.Identity.Client;

    /// <summary>
    /// メール送信ユーティリティ
    /// </summary>
    public static class AbpfMailUtil
    {
        /// <summary>
        /// メール送信① 送信元メールアドレス可変（社員用、AAD OAuth認証EWS方式）
        /// </summary>
        /// <param name="mailFrom">送信元アドレス</param>
        /// <param name="mailTo">送信先アドレス（複数）</param>
        /// <param name="mailCc">CCアドレス（複数）</param>
        /// <param name="mailBcc">BCCアドレス（複数）</param>
        /// <param name="subject">タイトル</param>
        /// <param name="body">本体</param>
        /// <param name="userInfo">ユーザー情報</param>
        /// <param name="memberName">メンバー名（呼び出し側は指定しない）</param>
        /// <param name="filePath">ファイル名（呼び出し側は指定しない）</param>
        /// <param name="lineNumber">行番号（呼び出し側は指定しない）</param>
        /// <returns>結果</returns>
        public static AbpfConstants.MailResult SendMail(
            string mailFrom,
            string[] mailTo,
            string[] mailCc,
            string[] mailBcc,
            string subject,
            string body,
            UserInfo userInfo,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = -1)
        {
            try
            {
                // 設定情報
                // アプリケーションID（クライアントID）
                string clientId = AbpfConfigUtil.GetValue(AbpfConfigUtil.SectionMail, AbpfConfigUtil.ClientId);

                // シークレット（暗号化）
                string clientSecret = Util.AbpfCryptUtil.Decrypt(AbpfConfigUtil.GetValue(AbpfConfigUtil.SectionMail, AbpfConfigUtil.ClientSecret));

                // テナントID
                string tenantId = AbpfConfigUtil.GetValue(AbpfConfigUtil.SectionMail, AbpfConfigUtil.TenantId);

                // EWSScope
                string[] ewsScopes = { AbpfConfigUtil.GetValue(AbpfConfigUtil.SectionMail, AbpfConfigUtil.EwsScopes) };

                // EWSUrl
                string ewsUrl = AbpfConfigUtil.GetValue(AbpfConfigUtil.SectionMail, AbpfConfigUtil.EwsUrl);

                // トークン取得
                IConfidentialClientApplication app = ConfidentialClientApplicationBuilder
                        .Create(clientId)
                        .WithAuthority(AzureCloudInstance.AzurePublic, tenantId)
                        .WithClientSecret(clientSecret)
                        .Build();
                Task<Microsoft.Identity.Client.AuthenticationResult> authenticationResult = app.AcquireTokenForClient(ewsScopes).ExecuteAsync();
                authenticationResult.Wait();

                Console.WriteLine("Received access token: \n" + authenticationResult.Result.AccessToken);

                // 送信設定
                ExchangeService service = new ExchangeService();
                service.Credentials = new OAuthCredentials(authenticationResult.Result.AccessToken);
                service.Url = new Uri(ewsUrl);
                service.ImpersonatedUserId = new ImpersonatedUserId(ConnectingIdType.SmtpAddress, mailFrom);   // 偽装設定

                service.HttpHeaders.Add("X-AnchorMailbox", mailFrom);

                // コンテンツ
                EmailMessage message = new EmailMessage(service)
                {
                    Subject = subject,
                    Body = body,
                };

                // 送信先メールアドレス
                foreach (string to in mailTo)
                {
                    message.ToRecipients.Add(to);
                }

                foreach (string cc in mailCc)
                {
                    message.CcRecipients.Add(cc);
                }

                foreach (string bcc in mailBcc)
                {
                    message.BccRecipients.Add(bcc);
                }

                // メール送信
                message.Send();

                return AbpfConstants.MailResult.Success;
            }
            catch (Exception e)
            {
                // 送信エラーログ
                AbpfLogUtil.LogError(e, userInfo, memberName, filePath, lineNumber);
                return AbpfConstants.MailResult.Error;
            }
        }

        /// <summary>
        /// メール送信② 送信元メールアドレス固定１（オンライン用、smtp.office365.com認証方式）
        /// </summary>
        /// <param name="mailTo">送信先アドレス（複数）</param>
        /// <param name="mailCc">CCアドレス（複数）</param>
        /// <param name="mailBcc">BCCアドレス（複数）</param>
        /// <param name="subject">タイトル</param>
        /// <param name="body">本体</param>
        /// <param name="userInfo">ユーザー情報</param>
        /// <param name="memberName">メンバー名（呼び出し側は指定しない）</param>
        /// <param name="filePath">ファイル名（呼び出し側は指定しない）</param>
        /// <param name="lineNumber">行番号（呼び出し側は指定しない）</param>
        /// <returns>結果</returns>
        public static AbpfConstants.MailResult SendMailFixed(
            string[] mailTo,
            string[] mailCc,
            string[] mailBcc,
            string subject,
            string body,
            UserInfo userInfo,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = -1)
        {
            try
            {
                // 設定情報
                // メールアカウント
                string accountId = AbpfConfigUtil.GetValue(AbpfConfigUtil.SectionMail, AbpfConfigUtil.AccountId);

                // パスワード（暗号化）
                string password = Util.AbpfCryptUtil.Decrypt(AbpfConfigUtil.GetValue(AbpfConfigUtil.SectionMail, AbpfConfigUtil.Password));

                // 送信元（固定）
                string fromAddress = AbpfConfigUtil.GetValue(AbpfConfigUtil.SectionMail, AbpfConfigUtil.FromAddress);

                // メールサーバ
                string hostname = AbpfConfigUtil.GetValue(AbpfConfigUtil.SectionMail, AbpfConfigUtil.Hostname);

                // ポート
                int port = AbpfCommonUtil.GetInt(AbpfConfigUtil.GetValue(AbpfConfigUtil.SectionMail, AbpfConfigUtil.Port));

                var mailServer = new SmtpClient();

                // DefaultCredentials が要求と共に送信されるかどうかを制御する
                mailServer.UseDefaultCredentials = false;

                // ユーザー名とパスワードを設定する
                mailServer.Credentials = new NetworkCredential(accountId, password);

                // SMTPサーバーなどを設定する
                mailServer.Host = hostname;

                // 拡張保護を使用するときの認証に使用するサービス プロバイダー名
                // _mailServer.TargetName = "STARTTLS/smtp.office365.com"; // same behaviour if this lien is removed
                mailServer.Port = port;

                // 接続を暗号化するために SSL (Secure Sockets Layer) を使用するかどうかを指定
                mailServer.EnableSsl = true;

                var eml = new MailMessage();
                eml.From = new MailAddress(fromAddress);

                // 送信先メールアドレス
                foreach (string to in mailTo)
                {
                    eml.To.Add(to);
                }

                foreach (string cc in mailCc)
                {
                    eml.CC.Add(cc);
                }

                foreach (string bcc in mailBcc)
                {
                    eml.Bcc.Add(bcc);
                }

                // コンテンツ
                eml.Subject = subject;
                eml.Body = body;

                mailServer.Send(eml);

                return AbpfConstants.MailResult.Success;
            }
            catch (Exception e)
            {
                // 送信エラーログ
                AbpfLogUtil.LogError(e, userInfo, memberName, filePath, lineNumber);
                return AbpfConstants.MailResult.Error;
            }
        }
    }
}
