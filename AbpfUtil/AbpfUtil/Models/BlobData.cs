// <copyright file="BlobData.cs" company="Above Beyond Project Framework">
// Copyright (c) Above Beyond Project Framework. All rights reserved.
// </copyright>

namespace AbpfUtil.Models
{
    using AbpfBase.Models;
    using System;

    /// <summary>
    /// BlobData.
    /// </summary>
    public class BlobData : ResultBaseModel
    {
        /// <summary>
        /// Gets or sets FileName.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets LastModified.
        /// </summary>
        public DateTime? LastModified { get; set; }
    }
}