// <copyright file="BasisTableData.cs" company="Above Beyond Project Framework">
// Copyright (c) Above Beyond Project Framework. All rights reserved.
// </copyright>

namespace Sample03Basis.Models
{
    using AbpfBase.Models;

    public class BasisTableData : ResultBaseModel
    {
        public string JobNo { get; set; }

        public string EstNo { get; set; }

        public string EstNoEda { get; set; }

        public string BkgDate { get; set; }

        public string BkMemo { get; set; }

        public string WorkUserId { get; set; }

        public string DestCd { get; set; }
    }
}