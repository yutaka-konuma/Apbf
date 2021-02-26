// <copyright file="BotSampleRecognizer.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Microsoft.BotBuilderSamples
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Bot.Builder;
    using Microsoft.Bot.Builder.AI.Luis;
    using Microsoft.Extensions.Configuration;

    public class BotSampleRecognizer : IRecognizer
    {
        private readonly LuisRecognizer luisRecognizer;

        /// <summary>
        /// Initializes a new instance of the <see cref="BotSampleRecognizer"/> class.
        /// </summary>
        /// <param name="configuration">configuration.</param>
        public BotSampleRecognizer(IConfiguration configuration)
        {
            var luisIsConfigured = !string.IsNullOrEmpty(configuration["LuisAppId"]) && !string.IsNullOrEmpty(configuration["LuisAPIKey"]) && !string.IsNullOrEmpty(configuration["LuisAPIHostName"]);
            if (luisIsConfigured)
            {
                var luisApplication = new LuisApplication(
                    configuration["LuisAppId"],
                    configuration["LuisAPIKey"],
                    "https://" + configuration["LuisAPIHostName"]);

                var recognizerOptions = new LuisRecognizerOptionsV3(luisApplication)
                {
                    PredictionOptions = new Bot.Builder.AI.LuisV3.LuisPredictionOptions
                    {
                        IncludeInstanceData = true,
                    },
                };

                this.luisRecognizer = new LuisRecognizer(recognizerOptions);
            }
        }

        public virtual bool IsConfigured => this.luisRecognizer != null;

        /// <inheritdoc/>
        public virtual async Task<RecognizerResult> RecognizeAsync(ITurnContext turnContext, CancellationToken cancellationToken)
            => await this.luisRecognizer.RecognizeAsync(turnContext, cancellationToken);

        /// <inheritdoc/>
        public virtual async Task<T> RecognizeAsync<T>(ITurnContext turnContext, CancellationToken cancellationToken)
            where T : IRecognizerConvert, new()
            => await this.luisRecognizer.RecognizeAsync<T>(turnContext, cancellationToken);
    }
}
