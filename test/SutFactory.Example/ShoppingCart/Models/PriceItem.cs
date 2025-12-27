namespace Hj.SutFactory.Example.ShoppingCart.Models;

/// <summary>
/// A model class representing a price. Typically this would be a lookup in
/// a "price book" that may be market or buyer specific.
/// </summary>
/// <param name="sku">The stock keeping unit (SKU) identifier for the item.</param>
/// <param name="price">The monetary price associated with the specified SKU.</param>
public record PriceItem(string sku, decimal price);
