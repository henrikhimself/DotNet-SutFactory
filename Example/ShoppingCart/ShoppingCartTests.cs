namespace Hj.SutFactory.Example.ShoppingCart;

public class ShoppingCartTests
{
  [Fact]
  public void AddItem_GivenSkuAndQuantity_AddsItem()
  {
    // arrange
    string? result = null;

    var sut = SystemUnderTest.For<ShoppingCartService>(arrange =>
    {
      SetHappyPath(arrange);

      // Add spy on the Create method to record the result. In this unit test, there is no need to configure
      // the IDataRepository ahead of creating the SUT. Instances for service types with no configured input
      // builder are automatically created as Singletons, so they can be retrieved via the Service Provider.
      arrange
        .Instance<IDataRepository>()
        .When(x => x.Create(Arg.Any<string>(), Arg.Any<IConvertible>()))
        .Do(x => result = x.ArgAt<string>(1));
    });

    // act
    sut.AddItem("sku123", 4);

    // assert
    Assert.Equal("""{"sku":"sku123","quantity":4}""", result);
  }

  private static void SetHappyPath(InputBuilder inputBuilder)
  {
    // Specifying an implementation here will create a concrete instance of the IPriceCalculator. This
    // increases the depth of unit tests to also include code in this implementation.
    inputBuilder.Instance<PriceCalculator>();

    // Getting the tax calculator instance via the service provider is a bit backwards but we can
    // configure it this way. The recommended way is to first configure the ITaxCalculator instance
    // and then configure the IPriceCalculator configuration.
    var taxCalculator = inputBuilder.GetRequiredService<ITaxCalculator>();
    taxCalculator.GetRate().Returns(0.10m);
  }
}
