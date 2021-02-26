// <copyright file="BasisCargoData.cs" company="Above Beyond Project Framework">
// Copyright (c) Above Beyond Project Framework. All rights reserved.
// </copyright>

namespace Sample03Basis.Models
{
    using AbpfBase.Models;
    using System.Collections.Generic;

    /// <summary>
    /// BasisCargo更新画面データ
    /// </summary>
    public class BasisCargoData : ResultBaseModel
    {
        public string JobNo { get; set; }

        public string EstNo { get; set; }

        public string EstNoEda { get; set; }

        public List<CargoTableData> ItemsEdited { get; set; }

        public List<CargoTableData> ItemsAdded { get; set; }

        public List<CargoTableData> ItemsRemoved { get; set; }
    }
}