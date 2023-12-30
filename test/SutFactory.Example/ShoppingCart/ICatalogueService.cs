using Hj.SutFactory.Example.ShoppingCart.Models;

namespace Hj.SutFactory.Example.ShoppingCart;

public interface ICatalogueService
{
  IEnumerable<CatalogueItem> GetCatalogueItems();
}
