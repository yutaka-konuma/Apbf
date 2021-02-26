// <copyright file="ChatSampleHubService.cs" company="Above Beyond Project Framework">
// Copyright (c) Above Beyond Project Framework. All rights reserved.
// </copyright>

namespace AbpfProto.Service
{
    using AbpfUtil.Models;
    using Microsoft.Extensions.Logging;
    using Sample05Chat.Repository;

    /// <summary>
    /// Abpf基盤サービス
    /// </summary>
    public class ChatSampleHubService : AbpfChatHubServiceBase
    {
        private readonly ILoggerFactory loggerFactory;
        private readonly ChatSampleRepository chatSampleRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChatSampleHubService"/> class.
        /// </summary>
        /// <param name="loggerFactory">a.</param>
        public ChatSampleHubService(
            ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            this.loggerFactory = loggerFactory;
            if (this.chatSampleRepository == null)
            {
                this.chatSampleRepository = new ChatSampleRepository(this.sqlConnection);
            }
        }

        public void InsertChatLog(string chatGroupId, string chatMessage, UserInfo userInfo)
        {
            this.chatSampleRepository.InsertChatLog(chatGroupId, chatMessage, userInfo);
        }

        public void AddToGroup(string chatGroupId, UserInfo userInfo)
        {
            this.chatSampleRepository.AddToGroup(chatGroupId, userInfo);
        }
    }
}
