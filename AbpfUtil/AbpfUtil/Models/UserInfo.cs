// <copyright file="UserInfo.cs" company="Above Beyond Project Framework">
// Copyright (c) Above Beyond Project Framework. All rights reserved.
// </copyright>

namespace AbpfUtil.Models
{
    using AbpfBase.Models;

    /*
     * サンプル
     * auth_time:1608686345
     * name:サンプル 太郎
     * mailAddress:xxxxxx@xxxx.xx.xx
     * country:日本
     * acceptLanguage:ja
     * tfp:B2C_1_signin1
     * utid:7884fec3-0ac2-4a6c-9953-ad15f58283f7
     * uid:b9853ae7-a908-45db-b4ef-2989c6cf5f59-b2c_1_signupsignin1
     */

    public class UserInfo : ResultBaseModel
    {

        public string UserIdentifier { get; set; }

        public string Name { get; set; }

        public string MailAddress { get; set; }

        public string Tfp { get; set; }

        public string UtId { get; set; }

        public string UId { get; set; }

        public string Country { get; set; }

        public string AcceptLanguage { get; set; }
    }
}
