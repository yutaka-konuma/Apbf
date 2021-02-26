// <copyright file="AbpfDialogBase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AbpfBotBase.Dialogs
{
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Extensions.Logging;

    // チャットボットダイアログ 基盤共通クラス
    public class AbpfDialogBase : ComponentDialog
    {

        protected readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AbpfDialogBase"/> class.
        /// </summary>
        /// <param name="luisRecognizer">luisRecognizer.</param>
        /// <param name="logger">logger.</param>
        public AbpfDialogBase(
            ILogger<AbpfDialogBase> logger)
            : base(nameof(AbpfDialogBase))
        {
            this.logger = logger;

            // テキストプロンプト準備
            this.AddDialog(new TextPrompt(nameof(TextPrompt)));

            this.InitialDialogId = nameof(WaterfallDialog);
        }

    }
}
