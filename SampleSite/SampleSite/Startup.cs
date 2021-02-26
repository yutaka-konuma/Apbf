// <copyright file="Startup.cs" company="Above Beyond Project Framework">
// Copyright (c) Above Beyond Project Framework. All rights reserved.
// </copyright>

namespace SampleSite
{
    using System.Text.Encodings.Web;
    using System.Text.Unicode;
    using Microsoft.AspNetCore.Authentication.OpenIdConnect;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Authorization;
    using Microsoft.AspNetCore.Mvc.Razor;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.WebEncoders;
    using Microsoft.Identity.Web;
    using Microsoft.Identity.Web.UI;
    using AbpfBase.Filter;
    using Sample05Chat.Hubs;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // ↓ Azure Active Directory B2C 連携
            services.AddMicrosoftIdentityWebAppAuthentication(this.Configuration, "AzureAdB2C")
                    .EnableTokenAcquisitionToCallDownstreamApi(new string[] { })
                    .AddInMemoryTokenCaches();

            services.AddControllersWithViews(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                options.Filters.Add(new AuthorizeFilter(policy));

                // ↓　ActionFilterを呼び出す
                options.Filters.Add(typeof(AbpfActionFilter));

                // ↑　ActionFilterを呼び出す
            }).AddMicrosoftIdentityUI();

            services.Configure<OpenIdConnectOptions>(this.Configuration.GetSection("AzureAdB2C"));

            // ↑ Azure Active Directory B2C 連携

            // ↓ HTMLのエンコーディングをUTF-8にする(日本語のHTMLエンコード防止)
            services.Configure<WebEncoderOptions>(options =>
            {
                options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All);
            });

            // ↑ HTMLのエンコーディングをUTF-8にする(日本語のHTMLエンコード防止)

            // ↓ チャット用、プッシュ通信部品
            services.AddSignalR();

            // ↑ チャット用、プッシュ通信部品

            // ↓ 多言語対応
            services.AddLocalization(options => options.ResourcesPath = "Language");

            services.AddMvc()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization();

            services.AddPortableObjectLocalization();

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[] { "en", "ja", "zh", "ko" };
                options.SetDefaultCulture(supportedCultures[0])
                    .AddSupportedCultures(supportedCultures)
                    .AddSupportedUICultures(supportedCultures);
            });

            // ↑ 多言語対応
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            // ↓ 多言語対応
            var supportedCultures = new[] { "en", "ja", "zh", "ko" };
            var localizationOptions = new RequestLocalizationOptions().SetDefaultCulture(supportedCultures[0])
                .AddSupportedCultures(supportedCultures)
                .AddSupportedUICultures(supportedCultures);

            app.UseRequestLocalization(localizationOptions);

            app.UseRouting();
            app.UseStaticFiles();

            app.UseAuthentication();

            // ↑ 多言語対応

            // ↓ Azure Active Directory B2C 連携
            app.UseAuthorization();

            // ↑ Azure Active Directory B2C 連携
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();

                // ↓ チャット用、プッシュ通信部品
                endpoints.MapHub<ChatHub>("/chathub");

                // ↑ チャット用、プッシュ通信部品
            });

            app.UseRequestLocalization();
        }
    }
}