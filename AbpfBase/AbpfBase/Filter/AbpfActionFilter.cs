// <copyright file="AbpfActionFilter.cs" company="Above Beyond Project Framework">
// Copyright (c) Above Beyond Project Framework. All rights reserved.
// </copyright>

namespace AbpfBase.Filter
{
    using System;
    using AbpfBase.Controllers;
    using AbpfBase.Models;
    using AbpfUtil.Models;
    using AbpfUtil.Util;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Filters;

    /// <summary>
    /// AbpfActionFilter
    /// </summary>
    public class AbpfActionFilter : IActionFilter
    {
        private DateTime startTime;

        /// <summary>
        /// actionメソッドが呼び出される前に呼び出されます。
        /// </summary>
        /// <param name="context">現在のリクエストとアクションに関する情報。</param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.Controller is AbpfController)
            {
                var c = context.Controller as AbpfController;
                this.startTime = DateTime.Now;
                AbpfLogUtil.LogInfoBase(c.ControllerContext.ActionDescriptor.ActionName, c.ControllerContext.ActionDescriptor.ControllerName, "開始", this.GetUserInfo(context.HttpContext), this.startTime);
            }

            if (context.Controller is AbpfControllerBase)
            {
                var c = context.Controller as AbpfControllerBase;
                this.startTime = DateTime.Now;
                AbpfLogUtil.LogInfoBase(c.ControllerContext.ActionDescriptor.ActionName, c.ControllerContext.ActionDescriptor.ControllerName, "開始", this.GetUserInfo(context.HttpContext), this.startTime);
            }
        }

        /// <summary>
        /// actionメソッドが呼び出された後に呼び出されます。
        /// </summary>
        /// <param name="context">現在のリクエストとアクションに関する情報。</param>
        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Controller is AbpfController)
            {
                var c = context.Controller as AbpfController;
                DateTime endTime = DateTime.Now;
                AbpfLogUtil.LogInfoBase(c.ControllerContext.ActionDescriptor.ActionName, c.ControllerContext.ActionDescriptor.ControllerName, "終了 経過時間" + (endTime - this.startTime).ToString(), this.GetUserInfo(context.HttpContext), endTime);
            }

            if (context.Controller is AbpfControllerBase)
            {
                var c = context.Controller as AbpfControllerBase;
                DateTime endTime = DateTime.Now;
                AbpfLogUtil.LogInfoBase(c.ControllerContext.ActionDescriptor.ActionName, c.ControllerContext.ActionDescriptor.ControllerName, "終了 経過時間" + (endTime - this.startTime).ToString(), this.GetUserInfo(context.HttpContext), endTime);
            }
        }

        /// <summary>
        /// ユーザー情報取得
        /// </summary>
        /// <param name="httpContext">Context</param>
        /// <returns>ユーザー情報</returns>
        private UserInfo GetUserInfo(HttpContext httpContext)
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
