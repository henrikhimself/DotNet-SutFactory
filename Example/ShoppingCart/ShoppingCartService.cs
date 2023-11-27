using System.Text.Json;

namespace Hj.SutFactory.Example.ShoppingCart;

public class ShoppingCartService(
  IPriceCalculator priceCalculator,
  IDataRepository dataRepository)
{
  private const string StoreName = "my shopping cart store";

  private readonly IPriceCalculator _priceCalculator = priceCalculator;
  private readonly IDataRepository _dataRepository = dataRepository;

  public void AddItem(string sku, int quantity)
  {
    var value = JsonSerializer.Serialize(new { sku, quantity });
    _dataRepository.Create(StoreName, value);
  }

  public (string sku, int quantity, decimal price) GetItems()
  {
    // var price = _priceCalculator.GetPrice(sku, quantity);
    return default;
  }

  public void UpdateItem()
  {
  }

  public void RemoveItem()
  {
  }
}
