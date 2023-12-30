namespace Hj.SutFactory.Example.ShoppingCart.Models;

/// <summary>
/// A model class representing a cart item. It pairs a SKU with the selected
/// quantity of that SKU such that product details can be looked up and a total
/// price can be calculated.
/// </summary>
public class CartItem
{
  /// <summary>
  /// Gets or sets the data store Id.
  /// </summary>
  public Guid? Id { get; set; }

  public decimal Quantity { get; set; }

  public required string Sku { get; init; }
}
