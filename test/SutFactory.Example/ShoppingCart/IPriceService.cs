using Hj.SutFactory.Example.ShoppingCart.Models;

namespace Hj.SutFactory.Example.ShoppingCart;

public interface IPriceService
{
  IEnumerable<PriceItem> GetPriceItems();
}
