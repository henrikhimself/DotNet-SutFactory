namespace Hj.SutFactory.Example.ShoppingCart.Models;

/// <summary>
/// A model class representing a SKU ie. a stock keeping unit in the shop
/// catalogue of items for sale. This is typically a variant of a product.
/// </summary>
/// <param name="Sku"></param>
/// <param name="Name"></param>
public record CatalogueItem(string Sku, string Name);
