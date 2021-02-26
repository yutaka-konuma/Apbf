// <copyright file="SearchSampleViewData.cs" company="Above Beyond Project Framework">
// Copyright (c) Above Beyond Project Framework. All rights reserved.
// </copyright>

namespace Search01Search.Models
{
    using AbpfBase.Models;
    using System.ComponentModel.DataAnnotations;

    public class SearchSampleViewData : ResultBaseModel
    {
        [Required]
        [StringLength(7)]
        public string Postcd { get; set; }

        [Required]
        [StringLength(10)]
        public string Country { get; set; }

        [Required]
        [StringLength(20)]
        public string City { get; set; }

        [Required]
        [StringLength(20)]
        public string Address { get; set; }
    }
}