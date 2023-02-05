using Hj.SutFactory.Registries.Implementation;

namespace Hj.SutFactory.UnitTest;

public class RegistryKeyGeneratorTests
{
  [Fact]
  public void GenerateKey_GivenObjectType_ReturnsNull()
  {
    // arrange
    var sut = new RegistryKeyGenerator();

    var type = typeof(object);

    // act
    var key = sut.GenerateKey(type);

    // assert
    Assert.Null(key);
  }

  [Fact]
  public void GenerateKey_GivenRuntimeType_ReturnsNull()
  {
    // arrange
    var sut = new RegistryKeyGenerator();

    var type = typeof(object).GetType();

    // act
    var key = sut.GenerateKey(type);

    // assert
    Assert.Null(key);
  }
}
