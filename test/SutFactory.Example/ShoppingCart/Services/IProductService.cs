using Hj.SutFactory.Example.ShoppingCart.Models;

namespace Hj.SutFactory.Example.ShoppingCart.Services;

public interface IProductService
{
  /// <summary>
  /// Gets a list of products.
  /// </summary>
  /// <returns>A list of product items.</returns>
  IEnumerable<ProductItem> GetProducts();
}
