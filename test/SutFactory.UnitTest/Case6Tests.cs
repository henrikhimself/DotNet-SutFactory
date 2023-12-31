using Hj.SutFactory.TestCase.Case6;

namespace Hj.SutFactory.UnitTest.Case6;

// Case 6 focus on resolving implementations from an external service provider.
public class Case6Tests
{
  [Fact]
  public void CreateSut_GivenExternalServiceProvider_CreatesSut()
  {
    // arrange
    var externalServiceProvider = new ExternalServiceProvider();
    var sutBuilder = new SutBuilder(externalServiceProvider);

    var externalService = externalServiceProvider.GetService<ExternalService>();

    // act
    var result = sutBuilder.CreateSut<Case6Sut>();

    // assert
    Assert.Same(externalService, result.ExternalService);
  }
}
