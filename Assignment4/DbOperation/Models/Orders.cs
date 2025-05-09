﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace DbOperation.Models;

public partial class Orders
{
    public int orderId { get; set; }

    public int fkCustomerId { get; set; }

    public DateTime orderDate { get; set; }

    public decimal totalAmount { get; set; }

    public string status { get; set; }

    public string orderPreparationStatus { get; set; }

    public DateTime? createdDate { get; set; }

    public DateTime? updatedDate { get; set; }

    public string sUser { get; set; }

    public virtual ICollection<Billing> Billing { get; set; } = new List<Billing>();

    public virtual Customers fkCustomer { get; set; }
}