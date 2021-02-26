// <copyright file="DialogBot.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Microsoft.BotBuilderSamples.Bots
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Bot.Builder;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Schema;
    using Microsoft.Extensions.Logging;

    public class DialogBot<T> : ActivityHandler
        where T : Dialog
    {
        protected readonly Dialog Dialog;
        protected readonly BotState ConversationState;
        protected readonly BotState UserState;
        protected readonly ILogger Logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="DialogBot{T}"/> class.
        /// </summary>
        /// <param name="conversationState">conversationState.</param>
        /// <param name="userState">userState.</param>
        /// <param name="dialog">dialog.</param>
        /// <param name="logger">logger.</param>
        public DialogBot(ConversationState conversationState, UserState userState, T dialog, ILogger<DialogBot<T>> logger)
        {
            this.ConversationState = conversationState;
            this.UserState = userState;
            this.Dialog = dialog;
            this.Logger = logger;
        }

        /// <inheritdoc/>
        public override async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.OnTurnAsync(turnContext, cancellationToken);

            await this.ConversationState.SaveChangesAsync(turnContext, false, cancellationToken);
            await this.UserState.SaveChangesAsync(turnContext, false, cancellationToken);
        }

        /// <inheritdoc/>
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            await this.Dialog.RunAsync(turnContext, this.ConversationState.CreateProperty<DialogState>("DialogState"), cancellationToken);
        }

        /// <inheritdoc/>
        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await this.Dialog.RunAsync(turnContext, this.ConversationState.CreateProperty<DialogState>("DialogState"), cancellationToken);
                }
            }
        }

    }
}
