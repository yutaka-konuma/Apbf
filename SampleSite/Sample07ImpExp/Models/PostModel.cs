// <copyright file="SearchSampleViewData.cs" company="Above Beyond Project Framework">
// Copyright (c) Above Beyond Project Framework. All rights reserved.
// </copyright>

namespace Search01Search.Models
{
    using AbpfBase.Models;

    public class PostModel : ResultBaseModel
    {
        public string Postcd { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public string Address { get; set; }
    }
}