namespace Hj.SutFactory.Example.ShoppingCart.Models;

/// <summary>
/// A model class representing a product.
/// </summary>
public class ProductItem
{
  public required string Sku { get; init; }

  public required string Name { get; init; }

  public decimal Price { get; init; }
}
