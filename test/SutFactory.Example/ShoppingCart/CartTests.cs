using System.Text.Json;
using Hj.SutFactory.Example.ShoppingCart.Models;
using Hj.SutFactory.Example.ShoppingCart.Services;

namespace Hj.SutFactory.Example.ShoppingCart;

/// <summary>
/// These tests uses the SutBuilder in order to use fewer lambdas.
/// 
/// See the DataStore example for how the SystemUnderTest.For<T> extension method
/// can be used to simplify tests when lambdas are tolerated.
/// </summary>
public class CartTests
{
  private static readonly CatalogueItem _rock = new("A", "Rock");
  private static readonly CatalogueItem _paper = new("B", "Paper");
  private static readonly CatalogueItem _scissor = new("C", "Scissor");

  [Fact]
  public void TotalPrice_GivenItems_ReturnsTotalPrice()
  {
    // arrange
    var sutBuilder = new SutBuilder();
    SetHappyPath(sutBuilder.InputBuilder);
    var sut = sutBuilder.CreateSut<Cart>();

    // act
    var result = sut.TotalPrice;

    // assert
    Assert.Equal(14m, result);
  }

  [Fact]
  public void AddItem_GivenItem_SavesItem()
  {
    // arrange
    var sutBuilder = new SutBuilder();
    SetHappyPath(sutBuilder.InputBuilder);
    var sut = sutBuilder.CreateSut<Cart>();

    // Add spy on the Create method to capture saved JSON.
    var cartItemJsonSpy = string.Empty;
    sutBuilder
      .GetRequiredService<IDataRepository>()
      .When(call => call.Create(Arg.Any<string>(), Arg.Any<string>()))
      .Do(call => cartItemJsonSpy = call.ArgAt<string>(1));

    // act
    var result = sut.AddItem(new CartItem()
    {
      Sku = _rock.Sku,
      Quantity = 2m,
    });

    // assert
    var cartItem = JsonSerializer.Deserialize<CartItem>(cartItemJsonSpy);
    Assert.NotNull(cartItem);
    Assert.Equal(_rock.Sku, cartItem.Sku);
    Assert.Equal(2m, cartItem.Quantity);
  }

  [Fact]
  public void GetItems_GivenItems_ReturnsItems()
  {
    // arrange
    var sutBuilder = new SutBuilder();
    SetHappyPath(sutBuilder.InputBuilder);
    var sut = sutBuilder.CreateSut<Cart>();

    // act
    var result = sut.GetItems();

    // assert
    Assert.Equal(3, result.Count());
    Assert.Collection(
      result,
      cartItem1 =>
      {
        Assert.Equal(_rock.Sku, cartItem1.Sku);
        Assert.Equal(1m, cartItem1.Quantity);
      },
      cartItem2 =>
      {
        Assert.Equal(_paper.Sku, cartItem2.Sku);
        Assert.Equal(2m, cartItem2.Quantity);

      },
      cartItem3 =>
      {
        Assert.Equal(_scissor.Sku, cartItem3.Sku);
        Assert.Equal(3m, cartItem3.Quantity);
      });
  }

  [Fact]
  public void GetItems_GivenItemWithNoPrice_SkipsItem()
  {
    // arrange
    var sutBuilder = new SutBuilder();
    SetHappyPath(sutBuilder.InputBuilder);
    var sut = sutBuilder.CreateSut<Cart>();

    // Break the happy path by removing a price in the pricing list.
    sutBuilder
      .GetRequiredService<List<PriceItem>>()
      .RemoveAt(0);

    // act
    var result = sut.GetItems();

    // assert
    Assert.Equal(2, result.Count());
  }

  [Fact]
  public void GetItems_GivenNullItem_SkipsItem()
  {
    // arrange
    var sutBuilder = new SutBuilder();
    SetHappyPath(sutBuilder.InputBuilder);
    var sut = sutBuilder.CreateSut<Cart>();

    // Break the happy path by adding a null item into the list of cart items.
    sutBuilder
      .GetRequiredService<List<CartItem?>>()
      .Add(null);

    // act
    var result = sut.GetItems();

    // assert
    Assert.Equal(3, result.Count());
  }

  [Fact]
  public void Remove_GivenItem_RemovesItem()
  {
    // arrange
    var sutBuilder = new SutBuilder();
    SetHappyPath(sutBuilder.InputBuilder);
    var sut = sutBuilder.CreateSut<Cart>();

    // Retrieve the first valid item in the list of cart items configured in the happy path.
    var cartItem = sutBuilder
      .GetRequiredService<List<CartItem>>()
      .First(item => item?.Id is not null);

    // Add spy to capture the Id to be deleted.
    var idToBeDeletedSpy = Guid.Empty;
    sutBuilder
      .GetRequiredService<IDataRepository>()
      .When(call => call.Delete(Arg.Any<string>(), Arg.Any<Guid>()))
      .Do(call => idToBeDeletedSpy = call.ArgAt<Guid>(1));

    // act
    sut.RemoveItem(cartItem);

    // assert
    Assert.Equal(cartItem.Id, idToBeDeletedSpy);
  }

  private static void SetHappyPath(InputBuilder inputBuilder)
  {
    // Extend the test coverage by using the actual ProductService implementation. The SUT factory
    // is responsible for constructing the ProductService instance, utilizing the catalogue and
    // price service mocks defined here.
    var productService = inputBuilder.Instance<IProductService, ProductService>();

    // Seed the catalogue service with items for sale.
    var catalogueService = inputBuilder.Instance<ICatalogueService>();
    var catalogueItemList = new List<CatalogueItem>() { _rock, _paper, _scissor, };
    catalogueService.GetCatalogueItems().Returns(catalogueItemList);

    // Seed the price service with price items.
    var priceItemList = inputBuilder.Instance<List<PriceItem>>();
    priceItemList.Add(new PriceItem(_rock.Sku, 1m));
    priceItemList.Add(new PriceItem(_paper.Sku, 2m));
    priceItemList.Add(new PriceItem(_scissor.Sku, 3m));

    var priceService = inputBuilder.Instance<IPriceService>();
    priceService.GetPriceItems().Returns(priceItemList);

    // Seed the data repository with cart contents.
    var cartItemList = inputBuilder.Instance<List<CartItem?>>();
    cartItemList.Add(new CartItem() { Id = Guid.NewGuid(), Quantity = 1, Sku = _rock.Sku, });
    cartItemList.Add(new CartItem() { Id = Guid.NewGuid(), Quantity = 2, Sku = _paper.Sku, });
    cartItemList.Add(new CartItem() { Id = Guid.NewGuid(), Quantity = 3, Sku = _scissor.Sku, });

    var dataRepository = inputBuilder.Instance<IDataRepository>();
    dataRepository.Create(Arg.Any<string>(), Arg.Any<string>()).Returns(call => Guid.NewGuid());
    dataRepository.Read<string>(Arg.Any<string>()).Returns(call => cartItemList.Select(y => JsonSerializer.Serialize(y)));
  }
}
