namespace Hj.SutFactory.Example;

public interface IPriceCalculator
{
  decimal GetPrice(string sku, int quantity);
}
