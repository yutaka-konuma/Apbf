// <copyright file="BotController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Microsoft.BotBuilderSamples.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Bot.Builder;
    using Microsoft.Bot.Builder.Integration.AspNet.Core;

    [Route("api/messages")]
    [ApiController]
    public class BotController : ControllerBase
    {
        private readonly IBotFrameworkHttpAdapter botFrameworkHttpAdapter;
        private readonly IBot bot;

        /// <summary>
        /// Initializes a new instance of the <see cref="BotController"/> class.
        /// </summary>
        /// <param name="adapter">adapter.</param>
        /// <param name="bot">bot.</param>
        public BotController(IBotFrameworkHttpAdapter adapter, IBot bot)
        {
            this.botFrameworkHttpAdapter = adapter;
            this.bot = bot;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        [HttpPost]
        [HttpGet]
        public async Task PostAsync()
        {
            await this.botFrameworkHttpAdapter.ProcessAsync(this.Request, this.Response, this.bot);
        }
    }
}
