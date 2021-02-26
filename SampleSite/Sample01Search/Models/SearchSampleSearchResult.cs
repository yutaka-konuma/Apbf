// <copyright file="SearchSampleViewData.cs" company="Above Beyond Project Framework">
// Copyright (c) Above Beyond Project Framework. All rights reserved.
// </copyright>

namespace Search01Search.Models
{
    using AbpfBase.Models;
    using System.Collections.Generic;

    public class SearchSampleSearchResult : ResultBaseModel
    {
        public List<SearchSampleViewData> searchSampleViewDataList { get; set; }
    }

}