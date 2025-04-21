using System.Text.Json;
using Hj.SutFactory.Example.ShoppingCart.Models;
using Hj.SutFactory.Example.ShoppingCart.Services;

namespace Hj.SutFactory.Example.ShoppingCart;

public class Cart(
  IDataRepository dataRepository,
  IProductService productService) : ICart
{
  private const string StoreName = "CartStore";

  private readonly IDataRepository _dataRepository = dataRepository;
  private readonly IProductService _productService = productService;

  public decimal TotalPrice
  {
    get
    {
      var totalPrice = GetItems().Sum(cartItem =>
      {
        var price = 0m;
        if (TryGetProduct(cartItem, out var product))
        {
          price = cartItem.Quantity * product!.Price;
        }
        return price;
      });
      return totalPrice;
    }
  }

  public CartItem AddItem(CartItem cartItem)
  {
    var jsonItem = JsonSerializer.Serialize(cartItem);
    cartItem.Id = _dataRepository.Create(StoreName, jsonItem);
    return cartItem;
  }

  public IEnumerable<CartItem> GetItems()
  {
    var jsonItems = _dataRepository.Read<string>(StoreName);
    foreach (var json in jsonItems)
    {
      var cartItem = JsonSerializer.Deserialize<CartItem>(json);
      if (TryGetProduct(cartItem, out var _))
      {
        yield return cartItem!;
      }
    }
  }

  public void RemoveItem(CartItem cartItem)
  {
    if (cartItem?.Id is null)
    {
      return;
    }

    _dataRepository.Delete(StoreName, cartItem.Id.Value);
  }

  private bool TryGetProduct(CartItem? cartItem, out ProductItem? productItem)
  {
    productItem = null;

    if (cartItem is null)
    {
      return false;
    }

    productItem = _productService.GetProducts().SingleOrDefault(product => cartItem.Sku == product.Sku);
    return productItem is not null;
  }
}
