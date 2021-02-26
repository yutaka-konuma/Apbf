// <copyright file="ChatSampleController.cs" company="Above Beyond Project Framework">
// Copyright (c) Above Beyond Project Framework. All rights reserved.
// </copyright>

namespace Sample05Chat.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Sample01Base.Controllers;
    using Sample05Chat.Models;
    using Sample05Chat.Service;
    using System.Collections.Generic;

    [Route("api/[controller]")]
    [ApiController]
    public class ChatSampleAPIController : SampleAPIControllerBase
    {
        private ILoggerFactory logger;

        public ChatSampleAPIController(ILoggerFactory logger)
            : base(logger)
        {
            this.logger = logger;
        }

        // GET: api/ChatSampleAPI/GetChatGroup/
        [Route("GetChatGroup/")]
        [HttpGet]
        public List<ChatSampleViewData> GetChatGroup()
        {
            using (var chatSampleService = new ChatSampleService(this.logger))
            {
                var ret = chatSampleService.Search(this.GetUserInfo(this.HttpContext));
                return ret;
            }
        }

        // GET: api/ChatSampleAPI/GetChatLog/1
        [Route("GetChatLog/{groupId}")]
        [HttpGet]
        public List<ChatSampleSendData> GetChatLog(string groupId)
        {
            using (var chatSampleService = new ChatSampleService(this.logger))
            {
                var ret = chatSampleService.SearchLog(groupId, this.GetUserInfo(this.HttpContext));
                return ret;
            }
        }
    }
}
