// <copyright file="ChatSampleService.cs" company="Above Beyond Project Framework">
// Copyright (c) Above Beyond Project Framework. All rights reserved.
// </copyright>

namespace Sample05Chat.Service
{
    using System;
    using System.Collections.Generic;
    using AbpfUtil.Models;
    using AbpfUtil.Util;
    using Microsoft.Extensions.Logging;
    using Sample01Base.Service;
    using Sample05Chat.Models;
    using Sample05Chat.Repository;

    public class ChatSampleService : SampleServiceBase
    {
        private ChatSampleRepository chatSampleRepository;
        private ILoggerFactory loggerFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChatSampleService"/> class.
        /// </summary>
        /// <param name="logger">logger.</param>
        /// <param name="userInfo">userInfo.</param>
        public ChatSampleService(ILoggerFactory logger)
            : base(logger)
        {
            try
            {
                this.loggerFactory = logger;
                this.chatSampleRepository = new ChatSampleRepository(this.sqlConnection);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public List<ChatSampleViewData> Search(UserInfo userInfo)
        {
            AbpfLogUtil.LogInfo("ロググループ検索", userInfo);
            var ret = this.chatSampleRepository.SearchAllChatGroup(userInfo);
            return ret;
        }

        public List<ChatSampleSendData> SearchLog(string groupId, UserInfo userInfo)
        {
            AbpfLogUtil.LogInfo("過去ログ検索", userInfo);
            var ret = this.chatSampleRepository.SearchChatLog(groupId, userInfo);
            return ret;
        }
    }
}
