namespace Hj.SutFactory.Example.ShoppingCart;

public interface IPriceCalculator
{
  decimal GetPrice(string sku, int quantity);
}
