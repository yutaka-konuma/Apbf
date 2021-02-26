// <copyright file="ChatSampleController.cs" company="Above Beyond Project Framework">
// Copyright (c) Above Beyond Project Framework. All rights reserved.
// </copyright>

namespace Sample05Chat.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Sample01Base.Controllers;
    using Sample05Chat.Service;

    public class ChatSampleController : SampleController
    {
        private ILoggerFactory logger;

        public ChatSampleController(ILoggerFactory logger)
            : base(logger)
        {
            this.logger = logger;
        }

        // GET: ChatSampleController
        public ActionResult Index()
        {
            return this.View();
        }
    }
}
