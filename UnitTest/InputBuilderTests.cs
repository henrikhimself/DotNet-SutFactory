using Hj.SutFactory.UnitTest.TestData;
using Microsoft.Extensions.DependencyInjection;

#nullable disable

namespace Hj.SutFactory.UnitTest;

public class InputBuilderTests
{
  [Fact]
  public void Instance_GivenNull_ReturnsNullInstance()
  {
    // arrange
    var sutBuilder = new SutBuilder();
    var inputBuilder = sutBuilder.InputBuilder;

    inputBuilder.Instance<ClassInput>(null).Configure((instance, serviceProvider) => instance.Value = new object());

    // act
    var result = sutBuilder.ServiceProvider.GetService<ClassInput>();

    // assert
    Assert.Null(result);
  }
}
