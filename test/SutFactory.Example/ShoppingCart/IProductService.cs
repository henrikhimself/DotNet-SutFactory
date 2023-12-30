using Hj.SutFactory.Example.ShoppingCart.Models;

namespace Hj.SutFactory.Example.ShoppingCart;

public interface IProductService
{
  /// <summary>
  /// Gets a list of products.
  /// </summary>
  /// <returns></returns>
  IEnumerable<ProductItem> GetProducts();
}
