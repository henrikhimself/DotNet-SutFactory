using Hj.SutFactory.UnitTest.TestData;
using NSubstitute;

#nullable disable

namespace Hj.SutFactory.UnitTest;

public class SutBuilderTests
{
  [Fact]
  public void CreateSut_GivenT_ReturnsInstance()
  {
    // arrange
    var sutBuilder = new SutBuilder();

    var inputBuilder = sutBuilder.InputBuilder;
    inputBuilder.Instance<NullInput>(null);
    inputBuilder.AutoInstance<IAutoInterfaceInput, AutoImplementationInput>();
    inputBuilder.AutoInstance<AutoImplementationInput>();
    inputBuilder.Instance(() => new InstanceInput());

    // act
    var result = sutBuilder.CreateSut<TestSut>();

    // assert
    Assert.Null(result.NullInput);
    Assert.NotNull(result.InterfaceInputValue);
    Assert.NotNull(result.ImplementationInputValue);
    Assert.NotNull(result.ClassInputValue);
    Assert.NotNull(result.ClassSealedInputValue);
    Assert.NotNull(result.ClassAbstractInputValue);
    Assert.NotNull(result.NestedClassInputValue);
    Assert.NotNull(result.ClassGenericValue);
    Assert.NotNull(result.EnumerableInput);
    Assert.NotNull(result.AutoInterfaceInput);
    Assert.NotNull(result.AutoImplementationInput);
    Assert.NotNull(result.InstanceInput);
  }

  [Fact]
  public void CreateSut_GivenConfigurenWithTAndServiceProvider_ReturnsInstance()
  {
    // arrange
    var sutBuilder = new SutBuilder();

    IServiceProvider serviceProvider = null;

    var inputBuilder = sutBuilder.InputBuilder;
    inputBuilder.AutoInstance<IInterfaceInput>()
      .Configure((i, sp) =>
      {
        serviceProvider = sp;
        i.IsTrue().Returns(true);
      });

    var sut = sutBuilder.CreateSut<TestSut>();

    // act
    var result = sutBuilder.CreateSut<TestSut>();

    // assert
    Assert.NotNull(serviceProvider);
    Assert.True(result.InterfaceInputValue.IsTrue());
  }

  [Fact]
  public void CreateSut_GivenAmbiguousCtorInput_Throws()
  {
    // arrange
    var sutBuilder = new SutBuilder();

    // act & assert
    Assert.Throws<InvalidOperationException>(() => sutBuilder.CreateSut<TestAmbiguousCtorSut>());
  }

  [Fact]
  public void GetService_GivenType_ReturnsInstance()
  {
    // arrange
    var serviceProvider = new SutBuilder().ServiceProvider;

    // act
    var result = serviceProvider.GetService(typeof(TestSut));

    // assert
    Assert.NotNull(result);
    Assert.NotSame(result, serviceProvider.GetService(typeof(TestSut)));
  }
}
