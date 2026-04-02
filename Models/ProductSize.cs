using System;
using System.Collections.Generic;

namespace MilkTeaPOS.Models;

/// <summary>
/// Size variants (S/M/L) with different prices
/// </summary>
public partial class ProductSize
{
    public Guid Id { get; set; }

    public Guid ProductId { get; set; }

    public decimal Price { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Product Product { get; set; } = null!;
}
