// <copyright file="ChatHub.cs" company="Above Beyond Project Framework">
// Copyright (c) Above Beyond Project Framework. All rights reserved.
// </copyright>

namespace Sample05Chat.Hubs
{
    using System;
    using System.Text.Json;
    using System.Threading.Tasks;
    using AbpfProto.Service;
    using AbpfUtil.Models;
    using AbpfUtil.Util;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.Extensions.Logging;
    using Sample05Chat.Models;

    //[Authorize]
    public class ChatHub : Hub
    {
        private readonly ILoggerFactory loggerFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChatHub"/> class.
        /// </summary>
        /// <param name="logger">a.</param>
        public ChatHub(ILoggerFactory logger) //: base(logger)
        {
            this.loggerFactory = logger;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="groupId">groupId.</param>
        /// <param name="message">message.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        public Task SendMessageToGroup(string groupId, string message)
        {
            if (string.IsNullOrEmpty(groupId) == true || groupId.Trim().Length == 0)
            {
                throw new ArgumentException();
            }

            //UserInfo userInfo = this.GetUserInfo();
            //UserInfo userInfo = AbpfCommonUtil.GetUserInfo(this.Context);
            UserInfo userInfo = new UserInfo();

            foreach (var claim in this.Context.User.Claims)
            {
                switch (claim.Type)
                {
                    case "name":
                        userInfo.Name = claim.Value;
                        break;
                    case "emails":
                        userInfo.MailAddress = claim.Value;
                        break;
                    case "country":
                        userInfo.Country = claim.Value;
                        break;
                    case "tfp":
                        userInfo.Tfp = claim.Value;
                        break;
                    case "utid":
                        userInfo.UtId = claim.Value;
                        break;
                    case "uid":
                        userInfo.UId = claim.Value;
                        userInfo.UserIdentifier = claim.Value;
                        break;
                    default:
                        break;
                }
            }

            
            // ↓ 発言内容のDB登録
            var dat = new ChatSampleSendData
            {
                ChatUserId = userInfo.UId,
                ChatUserName = userInfo.Name,
                ChatConnectionId = this.Context.ConnectionId,
                ChatGroupId = groupId,
                ChatText = message,
            };
            string ret = JsonSerializer.Serialize(dat);

            using (ChatSampleHubService chatSampleHubService = new ChatSampleHubService(this.loggerFactory))
            {
                chatSampleHubService.InsertChatLog(groupId, message, userInfo);
            }
            // ↑ 発言内容のDB登録

            // ↓ 発言内容をチャットグループの参会者に対してプッシュ通信実行
            return this.Clients.Group(groupId).SendAsync("Send_" + groupId, ret);
            // ↑ 発言内容をチャットグループの参会者に対してプッシュ通信実行

        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="groupId">groupId.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        public async Task AddToGroup(string groupId)
        {
            // ↓ チャット グループへの参加登録
            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, groupId);
            // ↑ チャット グループへの参加登録

            // ↓ チャットの既読・未読管理の為のDB登録処理
            UserInfo userInfo = this.GetUserInfo();
            var dat = new ChatSampleSendData
            {
                ChatUserId = userInfo.UId,
                ChatUserName = userInfo.Name,
                ChatConnectionId = this.Context.ConnectionId,
                ChatGroupId = groupId,
                ChatText = $"Join groupId:{groupId}",
            };
            string ret = JsonSerializer.Serialize(dat);
            await this.Clients.Group(groupId).SendAsync("Send_" + groupId, ret);

            using (ChatSampleHubService chatSampleHubService = new ChatSampleHubService(this.loggerFactory))
            {
                chatSampleHubService.AddToGroup(groupId, userInfo);
            }
            // ↑ チャットの既読・未読管理の為のDB登録処理

        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="groupId">groupId.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        public async Task RemoveFromGroup(string groupId)
        {
            await this.Groups.RemoveFromGroupAsync(this.Context.ConnectionId, groupId);

            UserInfo userInfo = this.GetUserInfo();
            var dat = new ChatSampleSendData
            {
                ChatUserId = userInfo.UId,
                ChatUserName = userInfo.Name,
                ChatConnectionId = this.Context.ConnectionId,
                ChatGroupId = groupId,
                ChatText = $"Remove groupId:{groupId}",
            };
            string ret = JsonSerializer.Serialize(dat);
            await this.Clients.Group(groupId).SendAsync("Send_" + groupId, ret);
        }

        protected UserInfo GetUserInfo()
        {
            return AbpfCommonUtil.GetUserInfo(this.Context);
        }
    }
}
