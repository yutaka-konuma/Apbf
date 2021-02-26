// <copyright file="ChatSampleViewData.cs" company="Above Beyond Project Framework">
// Copyright (c) Above Beyond Project Framework. All rights reserved.
// </copyright>

using AbpfBase.Models;

namespace Sample05Chat.Models
{

    /// <summary>
    /// ChatSampleViewData.
    /// </summary>
    public class ChatSampleViewData : ResultBaseModel
    {
        /// <summary>
        /// Gets or sets chatGroupId.
        /// </summary>
        public string ChatGroupId { get; set; }

        /// <summary>
        /// Gets or sets chatGroupName.
        /// </summary>
        public string ChatGroupName { get; set; }
    }
}