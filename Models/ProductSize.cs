namespace MilkTeaPOS.Models;

/// <summary>
/// Size variants (S/M/L) with different prices
/// </summary>
public partial class ProductSize
{
    public Guid Id { get; set; }

    public Guid ProductId { get; set; }

    /// <summary>
    /// Size type: S, M, L (matches PostgreSQL ENUM size_type)
    /// </summary>
    public string Size { get; set; } = "M";

    public decimal Price { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Product Product { get; set; } = null!;
}
