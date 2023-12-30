using Hj.SutFactory.Example.ShoppingCart.Models;

namespace Hj.SutFactory.Example.ShoppingCart.Services;

public interface IPriceService
{
  IEnumerable<PriceItem> GetPriceItems();
}
