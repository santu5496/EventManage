﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace DbOperation.Models;

public partial class ItemsUsedForGoods
{
    public int itemId { get; set; }

    public int fkBakeRecId { get; set; }

    public int fkMasterItemId { get; set; }

    public decimal quantity { get; set; }

    public string unit { get; set; }

    public string goodsType { get; set; }

    public virtual InventoryItems fkMasterItem { get; set; }
}