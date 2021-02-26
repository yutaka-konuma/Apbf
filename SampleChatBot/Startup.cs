namespace SampleChatBot
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Bot.Builder;
    using Microsoft.Bot.Builder.Integration.AspNet.Core;
    using Microsoft.BotBuilderSamples;
    using Microsoft.BotBuilderSamples.Bots;
    using Microsoft.BotBuilderSamples.Dialogs;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public class Startup
    {
        // このメソッドはランタイムによって呼び出されます。 このメソッドを使用して、コンテナーにサービスを追加します。
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson();

            // エラー処理を有効にしてボットフレームワークアダプタを作成します。
            services.AddSingleton<IBotFrameworkHttpAdapter, AdapterWithErrorHandler>();

            // ユーザーと会話の状態に使用するストレージを作成します。
            services.AddSingleton<IStorage, MemoryStorage>();

            // ユーザー状態を作成します。 （このボットのDialog実装で使用されます。）
            services.AddSingleton<UserState>();

            // 会話状態を作成します。 （ダイアログシステム自体によって使用されます。）
            services.AddSingleton<ConversationState>();

            // LUISレコグナイザーを登録する
            services.AddSingleton<BotSampleRecognizer>();

            // ボットによって実行されるMainDialog。
            services.AddSingleton<MainDialog>();

            // ボットを一時的なものとして作成します。 （この場合、ASPコントローラーはIBotを想定しています。）
            services.AddTransient<IBot, DialogBot<MainDialog>>();
        }

        // このメソッドはランタイムによって呼び出されます。 このメソッドを使用して、HTTP要求パイプラインを構成します。
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseDefaultFiles()
                .UseStaticFiles()
                .UseWebSockets()
                .UseRouting()
                .UseAuthorization()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });
        }
    }
}
