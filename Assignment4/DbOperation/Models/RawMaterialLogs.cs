﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace DbOperation.Models;

public partial class RawMaterialLogs
{
    public int logId { get; set; }

    public int supplierId { get; set; }

    public DateTime createdDate { get; set; }

    public DateTime updatedDate { get; set; }

    public string sUser { get; set; }

    public DateTime purchaseDate { get; set; }

    public decimal totalAmount { get; set; }

    public decimal? advanceAmount { get; set; }

    public string paymentMode { get; set; }

    public string paymentStatus { get; set; }

    public virtual ICollection<PurchaseDetails> PurchaseDetails { get; set; } = new List<PurchaseDetails>();

    public virtual Suppliers supplier { get; set; }
}