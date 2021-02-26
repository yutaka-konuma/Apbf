// <copyright file="MainDialog.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Microsoft.BotBuilderSamples.Dialogs
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using AbpfProto.Service;
    using AbpfUtil.Models;
    using Microsoft.Bot.Builder;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Schema;
    using Microsoft.Extensions.Logging;
    using Sample03Basis.Models;
    using SampleBotBase.Dialogs;

    public class MainDialog : SampleDialogBase
    {
        // 自然言語認識Service
        private readonly BotSampleRecognizer botSampleRecognizer;

        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainDialog"/> class.
        /// </summary>
        /// <param name="luisRecognizer">luisRecognizer.</param>
        /// <param name="logger">logger.</param>
        public MainDialog(
            BotSampleRecognizer luisRecognizer,
            ILogger<MainDialog> logger)
            : base(logger)
        {
            // Service準備（LUIS)
            this.botSampleRecognizer = luisRecognizer;

            // チャットボットのフロー定義
            //  １．挨拶（IntroStepAsync）
            //  ２．やり取り（ActStepAsync） … 終了するまで繰り越す
            //  ３．終了（FinalStepAsync）
            this.AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                this.IntroStepAsync,
                this.ActStepAsync,
                this.FinalStepAsync,
            }));
        }

        // １．挨拶（IntroStepAsync）
        private async Task<DialogTurnResult> IntroStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            // 設定確認
            if (!this.botSampleRecognizer.IsConfigured)
            {
                await stepContext.Context.SendActivityAsync(
                    MessageFactory.Text("NOTE: LUIS is not configured. To enable all capabilities, add 'LuisAppId', 'LuisAPIKey' and 'LuisAPIHostName' to the appsettings.json file.", inputHint: InputHints.IgnoringInput), cancellationToken);

                return await stepContext.NextAsync(null, cancellationToken);
            }

            // あいさつ文生成
            var messageText = stepContext.Options?.ToString() ?? $"本日はどの様なご用件でしょうか？\n\n「【伝票番号】の〇〇を知りたい」等\n\n問い合わせを入力してください。";
            var promptMessage = MessageFactory.Text(messageText, messageText, InputHints.ExpectingInput);
            return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
        }

        // ２．やり取り（ActStepAsync） … 終了するまで繰り越す
        private async Task<DialogTurnResult> ActStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            // ↓ やり取りの開始 ＝ 利用者の発言を自然言語解析サービスに渡して解析する
            var luisResult = await this.botSampleRecognizer.RecognizeAsync<BotSampleModel>(stepContext.Context, cancellationToken);
            // ↑ やり取りの開始 ＝ 利用者の発言を自然言語解析サービスに渡して解析する

            UserInfo userInfo = new UserInfo();

            // 設定できるID ＝ チャットボットのinstanceIDを設定
            userInfo.UId = stepContext.ActiveDialog.State["instanceId"] != null && string.IsNullOrEmpty(stepContext.ActiveDialog.State["instanceId"].ToString()) == false ? stepContext.ActiveDialog.State["instanceId"].ToString() : "NoUser";
            userInfo.Name = "SampleChatBot";

            // ↓ 解析結果をロジックで処理する
            switch (luisResult.TopIntent().intent)
            {
                case BotSampleModel.Intent.SearchOrder:

                    // ↓ 返信文言を編集する
                    List<string> getWeatherMessageTextList = new List<string>();
                    string getWeatherMessageText = string.Empty;
                    StringBuilder strLog = new StringBuilder ("伝票番号検索を実行します");

                    List<BasisTableData> basisTableDataList = new List<BasisTableData>();
                    string selectItem = string.Empty;

                    if (luisResult.Entities != null) 
                    {
                        if (!string.IsNullOrEmpty(luisResult.Entities.OrderNumber[0]) && luisResult.Entities.OrderNumber[0].Length > 0)
                        {
                            // ↓ 検索処理を実行
                            using (BasisTableAPIService basisTableAPIService = new BasisTableAPIService(null))
                            {
                                // for(int i = 0; i <= luisResult.Entities.OrderNumber.Length; i++)
                                foreach (string orderNumber in luisResult.Entities.OrderNumber)
                                {
                                    strLog.AppendLine("\n\n伝票番号 : " + orderNumber);

                                    basisTableDataList.AddRange(basisTableAPIService.Get(orderNumber, userInfo));
                                }
                            }
                            // ↑ 検索処理を実行
                        }
                    }


                    if (luisResult.Entities.SelectItem != null && !string.IsNullOrEmpty(luisResult.Entities.SelectItem[0][0]) && luisResult.Entities.SelectItem[0][0].Length > 0)
                    {
                        selectItem = luisResult.Entities.SelectItem[0][0];
                        strLog.AppendLine("\n\n取得項目 : " + selectItem);
                    }

                    strLog.AppendLine("\n\n検索結果：");
                    if (basisTableDataList.Count > 0)
                    {
                        foreach (var item in basisTableDataList)
                        {
                            switch (selectItem)
                            {
                                case "日時":
                                    strLog.AppendLine("\n\n" + item.BkgDate);
                                    getWeatherMessageTextList.Add(@$"伝票番号：{item.JobNo}の{selectItem}は「{item.BkgDate}」です。");
                                    break;
                                case "備考":
                                    strLog.AppendLine("\n\n" + item.BkMemo);
                                    getWeatherMessageTextList.Add(@$"伝票番号：{item.JobNo}の{selectItem}は「{item.BkMemo}」です。");
                                    break;
                                case "担当者":
                                    strLog.AppendLine("\n\n" + item.WorkUserId);
                                    getWeatherMessageTextList.Add(@$"伝票番号：{item.JobNo}の{selectItem}は「{item.WorkUserId}」です。");
                                    break;
                                case "配送先":
                                    strLog.AppendLine("\n\n" + item.DestCd);
                                    getWeatherMessageTextList.Add(@$"伝票番号：{item.JobNo}の{selectItem}は「{item.DestCd}」です。");
                                    break;
                                default:
                                    strLog.AppendLine("\n\n表示項目を判断できませんでした");
                                    getWeatherMessageText = "表示項目を判断できませんでした";
                                    break;
                            }
                        }
                    }
                    else
                    {
                        strLog.AppendLine("\n\n該当する伝票はありませんでした");
                        getWeatherMessageText = "該当する伝票はありませんでした";
                    }

                    if (getWeatherMessageTextList.Count > 0)
                    {
                        getWeatherMessageText = string.Join("\n\n", getWeatherMessageTextList);
                    }
                    // ↑ 返信文言を編集する

                    Console.WriteLine(strLog.ToString());

                    // ↓ 編集した文言を返信する
                    var getWeatherMessage = MessageFactory.Text(getWeatherMessageText, getWeatherMessageText, InputHints.IgnoringInput);
                    await stepContext.Context.SendActivityAsync(getWeatherMessage, cancellationToken);
                    // ↑ 編集した文言を返信する

                    break;

                default:
                    var didntUnderstandMessageText = $"申し訳ありません\n\nお問い合わせに対する回答を行えませんでした\n\n (intent was {luisResult.TopIntent().intent})";
                    var didntUnderstandMessage = MessageFactory.Text(didntUnderstandMessageText, didntUnderstandMessageText, InputHints.IgnoringInput);
                    await stepContext.Context.SendActivityAsync(didntUnderstandMessage, cancellationToken);
                    break;
            }
            // ↑ 解析結果をロジックで処理する

            return await stepContext.NextAsync(null, cancellationToken);
        }

        // ３．終了（FinalStepAsync）
        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var promptMessage = "次のお問い合わせを入力してください";
            return await stepContext.ReplaceDialogAsync(this.InitialDialogId, promptMessage, cancellationToken);
        }
    }
}
