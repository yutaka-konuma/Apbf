// <copyright file="ChatSampleSendData.cs" company="Above Beyond Project Framework">
// Copyright (c) Above Beyond Project Framework. All rights reserved.
// </copyright>

using AbpfBase.Models;

namespace Sample05Chat.Models
{
    /// <summary>
    /// ChatSampleSendData.
    /// </summary>
    public class ChatSampleSendData : ResultBaseModel
    {
        /// <summary>
        /// Gets or sets groupId.
        /// </summary>
        public string ChatGroupId { get; set; }

        /// <summary>
        /// Gets or sets seqNo.
        /// </summary>
        public decimal ChatSeq { get; set; }

        /// <summary>
        /// Gets or sets connectionId.
        /// </summary>
        public string ChatConnectionId { get; set; }

        /// <summary>
        /// Gets or sets accountUId.
        /// </summary>
        public string ChatUserId { get; set; }

        /// <summary>
        /// Gets or sets accountName.
        /// </summary>
        public string ChatUserName { get; set; }

        /// <summary>
        /// Gets or sets message.
        /// </summary>
        public string ChatText { get; set; }

        /// <summary>
        /// Gets or sets ChatDate.
        /// </summary>
        public string ChatDate { get; set; }

        /// <summary>
        /// Gets or sets ChatReaded.
        /// </summary>
        public string ChatReaded { get; set; }

        /// <summary>
        /// Gets or sets ChatNoRead.
        /// </summary>
        public string ChatNoread { get; set; }
    }
}
