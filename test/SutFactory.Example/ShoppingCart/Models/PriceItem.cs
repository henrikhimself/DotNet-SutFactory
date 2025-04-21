namespace Hj.SutFactory.Example.ShoppingCart.Models;

/// <summary>
/// A model class representing a price. Typically this would be a lookup in
/// a "price book" that may be market or buyer specific.
/// </summary>
/// <param name="Sku"></param>
/// <param name="Price"></param>
public record PriceItem(string Sku, decimal Price);
