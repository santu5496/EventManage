﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace DbOperation.Models;

public partial class BillOrderItems
{
    public int itemId { get; set; }

    public int fkBillOrderId { get; set; }

    public int fkItemId { get; set; }

    public int quantity { get; set; }

    public string unit { get; set; }

    public decimal price { get; set; }

    public DateTime createdDate { get; set; }

    public DateTime updatedDate { get; set; }

    public string sUser { get; set; }

    public string entryType { get; set; }

    public string deliveredStatus { get; set; }

    public decimal? deliveredQty { get; set; }
}