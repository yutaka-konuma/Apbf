// <copyright file="CargoTableData.cs" company="Above Beyond Project Framework">
// Copyright (c) Above Beyond Project Framework. All rights reserved.
// </copyright>

namespace Sample03Basis.Models
{
    using AbpfBase.Models;

    public class CargoTableData : ResultBaseModel
    {
        public string JobNo { get; set; }

        public decimal CargoSeq { get; set; }

        public decimal NetWeight { get; set; }

        public string PgKbn { get; set; }

        public string FpointUnitKbn { get; set; }

        public string NosKbn { get; set; }

        public string MpKbn { get; set; }

        public string SysEntDate { get; set; }

        public string SysEntTime { get; set; }
    }
}