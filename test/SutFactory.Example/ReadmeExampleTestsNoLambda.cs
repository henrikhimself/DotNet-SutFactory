using Hj.SutFactory.Example.DataStore;
using Hj.SutFactory.Example.DataStore.Models;

namespace Hj.SutFactory.Example;

public class ReadmeExampleTestsNoLambda : ReadmeExampleTestsBase
{
  [Fact]
  public override void Read_GivenCompatibleDataStore_ReturnsAll()
  {
    // arrange
    var sutBuilder = new SutBuilder();

    SetHappyPath(sutBuilder.InputBuilder);

    var sut = sutBuilder.CreateSut<DataRepository>();

    // act
    var result = sut.Read<int>("my integer store");

    // assert
    Assert.Collection(result, item1 => Assert.Equal(10, item1));
  }

  [Fact]
  public override void Read_GivenIncompatibleDataStore_Throws()
  {
    // arrange
    var sutBuilder = new SutBuilder();

    SetHappyPath(sutBuilder.InputBuilder);

    // Breaking the happy path!
    // Get the list of data entities and modify it such that
    // a format exception will be thrown.
    var dataEntities = sutBuilder.GetRequiredService<List<DataEntity>>();
    dataEntities[0].Value = "this is not an integer";

    var sut = sutBuilder.CreateSut<DataRepository>();

    // assert
    Assert.Throws<FormatException>(() =>
    {
      // act
      sut.Read<int>("my integer store").ToList();
    });
  }
}
