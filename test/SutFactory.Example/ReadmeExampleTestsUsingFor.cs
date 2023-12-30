using Hj.SutFactory.Example.DataStore;
using Hj.SutFactory.Example.DataStore.Models;

namespace Hj.SutFactory.Example;

public class ReadmeExampleTestsUsingFor : ReadmeExampleTestsBase
{
  [Fact]
  public override void Read_GivenCompatibleDataStore_ReturnsAll()
  {
    // arrange
    var sut = SystemUnderTest.For<DataRepository>(SetHappyPath);

    // act
    var result = sut.Read<int>("my integer store");

    // assert
    Assert.Collection(result, item1 => Assert.Equal(10, item1));
  }

  [Fact]
  public override void Read_GivenIncompatibleDataStore_Throws()
  {
    // arrange
    var sut = SystemUnderTest.For<DataRepository>(arrange =>
    {
      SetHappyPath(arrange);

      // Breaking the happy path!
      // Get the list of data entities and modify it such that
      // a format exception will be thrown.
      var dataEntities = arrange.GetRequiredService<List<DataEntity>>();
      dataEntities[0].Value = "this is not an integer";
    });

    // assert
    Assert.Throws<FormatException>(() =>
    {
      // act
      sut.Read<int>("my integer store").ToList();
    });
  }
}
