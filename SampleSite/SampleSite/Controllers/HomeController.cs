// <copyright file="HomeController.cs" company="Above Beyond Project Framework">
// Copyright (c) Above Beyond Project Framework. All rights reserved.
// </copyright>

namespace SampleSite.Controllers
{
    using System;
    using System.Diagnostics;
    using AbpfUtil.Util;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Localization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Localization;
    using Microsoft.Extensions.Logging;
    using Microsoft.Identity.Web;
    using AbpfBase.Models;
    using Sample01Base.Controllers;

    // [Authorize]
    [AllowAnonymous]
    public class HomeController : SampleController
    {
        private readonly ITokenAcquisition tokenAcquisition;

        // private readonly ILogger<HomeController> _logger;
        // private readonly IStringLocalizer<HomeController> _localizer;
        private readonly IStringLocalizer localizer;

        private ILoggerFactory logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        /// <param name="tokenAcquisition">tokenAcquisition</param>
        /// <param name="factory">factory</param>
        /// <param name="logger">logger</param>
        public HomeController(
            ITokenAcquisition tokenAcquisition,
            IStringLocalizerFactory factory,
            ILoggerFactory logger)
            : base(logger)
        {
            this.tokenAcquisition = tokenAcquisition;

            // _localizer = localizer;
            var type = typeof(SharedResource);
            this.localizer = factory.Create(type);
            this.logger = logger;
        }

        public IActionResult Index()
        {
            this.ViewData["LocalizMessageDic"] = AbpfMessageUtil.GetLocalizMessageAll(this.localizer);
            this.ViewData["Message"] = this.localizer["ControllerMessage"];

            // _logger.LogInformation("Enter HomeController.Index Method");
            return this.View();
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            this.Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) });

            return this.LocalRedirect(returnUrl);
        }
    }
}