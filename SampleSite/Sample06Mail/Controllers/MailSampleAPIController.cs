// <copyright file="MailSampleAPIController.cs" company="Above Beyond Project Framework">
// Copyright (c) Above Beyond Project Framework. All rights reserved.
// </copyright>

namespace Sample06Mail.Controllers
{
    using AbpfUtil.Util;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Sample01Base.Controllers;

    /// <summary>
    /// メールサンプルAPIコントローラー
    /// </summary>
    [ApiController]
    public class MailSampleAPIController : SampleAPIControllerBase
    {
        private ILoggerFactory logger;

        public MailSampleAPIController(ILoggerFactory logger, IConfiguration configuration)
            : base(logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// メール送信サンプル(/Api/MailSampleAPI)
        /// </summary>
        /// <param name="number">1or2</param>
        /// <returns>正常ならMailResult.Successを返す</returns>
        [HttpPost("{number}")]
        public int PostAsync(string number)
        {
            if (number == "1")
            {
                string mailFrom = "yutaka_konuma@nrsgroup.co.jp";
                string[] mailTo = { "kenichi_sawai@nrsgroup.co.jp" };
                string[] mailCc = { };
                string[] mailBcc = { };
                string subject = "メール送信テスト①";
                string body = "メール送信テストです①";
                return (int)AbpfMailUtil.SendMail(mailFrom, mailTo, mailCc, mailBcc, subject, body, this.GetUserInfo(this.HttpContext));
            }

            if (number == "2")
            {
                string[] mailTo = { "kenichi_sawai@nrsgroup.co.jp" };
                string[] mailCc = { };
                string[] mailBcc = { };
                string subject = "メール送信テスト②";
                string body = "メール送信テストです②";
                return (int)AbpfMailUtil.SendMailFixed(mailTo, mailCc, mailBcc, subject, body, this.GetUserInfo(this.HttpContext));
            }

            return -9; // 番号エラー
        }

        /// <summary>
        /// 暗号化サンプル(/Api/MailSampleAPI)
        /// </summary>
        /// <param name="data">暗号化する文字列</param>
        /// <returns>正常なら暗号化結果</returns>
        [HttpGet("{data}")]
        public string Get(string data)
        {
            return AbpfCryptUtil.Encrypt(data);
        }
    }
}
