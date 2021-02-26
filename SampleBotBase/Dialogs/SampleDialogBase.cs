// <copyright file="AbpfDialogBase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AbpfBotBase.Dialogs;
using Microsoft.Extensions.Logging;

namespace SampleBotBase.Dialogs
{
    // チャットボットダイアログ 業務共通クラス
    public class SampleDialogBase : AbpfDialogBase
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="AbpfDialogBase"/> class.
        /// </summary>
        /// <param name="luisRecognizer">luisRecognizer.</param>
        /// <param name="logger">logger.</param>
        public SampleDialogBase(
            ILogger<AbpfDialogBase> logger)
            : base(logger)
        {

        }

    }
}
