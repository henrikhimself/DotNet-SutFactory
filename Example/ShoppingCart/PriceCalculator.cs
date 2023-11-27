namespace Hj.SutFactory.Example.ShoppingCart;

public class PriceCalculator(
  ITaxCalculator taxCalculator) : IPriceCalculator
{
  private readonly ITaxCalculator _taxCalculator = taxCalculator;

  public decimal GetPrice(string sku, int quantity)
  {
    var pricePerEaches = sku switch
    {
      "sku123" => 2m,
      _ => throw new InvalidDataException($"Missing price for SKU {sku}"),
    };
    var totalPrice = pricePerEaches * quantity * (1 + _taxCalculator.GetRate());
    return totalPrice;
  }
}
