namespace Hj.SutFactory.Example.ShoppingCart.Models;

/// <summary>
/// A model class representing a SKU ie. a stock keeping unit in the shop
/// catalogue of items for sale. This is typically a variant of a product.
/// </summary>
/// <param name="sku">The stock keeping unit identifier for this catalogue item.</param>
/// <param name="name">The display name of this catalogue item.</param>
public record CatalogueItem(string sku, string name);
