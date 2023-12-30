using Hj.SutFactory.Example.ShoppingCart.Models;

namespace Hj.SutFactory.Example.ShoppingCart.Services;

public class ProductService(
  ICatalogueService catalogueService,
  IPriceService priceService) : IProductService
{
  private readonly ICatalogueService _catalogueService = catalogueService;
  private readonly IPriceService _priceService = priceService;

  public IEnumerable<ProductItem> GetProducts()
  {
    var catalogueItems = _catalogueService.GetCatalogueItems();
    var priceItems = _priceService.GetPriceItems();

    foreach (var catalogueItem in catalogueItems)
    {
      var priceItem = priceItems.FirstOrDefault(x => x.Sku == catalogueItem.Sku);
      if (priceItem is null)
      {
        continue;
      }

      yield return new ProductItem()
      {
        Sku = catalogueItem.Sku,
        Name = catalogueItem.Name,
        Price = priceItem.Price,
      };
    }
  }
}
