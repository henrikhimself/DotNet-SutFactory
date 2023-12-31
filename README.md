# A SUT Factory.
This is a System Under Test (SUT) factory designed to eliminate the necessity for hard-coded parameter types during the creation of a SUT instance. It achieves this by integrating a service provider with the NSubstitute mocking library.

In its most basic form, the factory provides a single static method "SystemUnderTest.For" for arranging only the mocks that are relevant to you before the SUT is created. The "SutBuilder" provides more advanced methods and can be used for lambda-less coding or less usual arrangements e.g. testing implementations with multiple constructors.

An example of using the SystemUnderTest.For\<T\> method:
```cs
[Fact]
public void Read_GivenCompatibleDataStore_ReturnsAll()
{
  // arrange
  var sut = SystemUnderTest.For<DataRepository>(SetHappyPath);

  // act
  var result = sut.Read<int>("my integer store");

  // assert
  Assert.Collection(result, item1 => Assert.Equal(10, item1));
}

[Fact]
public void Read_GivenIncompatibleDataStore_Throws()
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
```

And this is the same example but using the SutBuilder to write (mostly) lambda-less code.

```cs
[Fact]
public void Read_GivenCompatibleDataStore_ReturnsAll()
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
public void Read_GivenIncompatibleDataStore_Throws()
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
```

This is the common "SetHappyPath" method referenced above.
```cs
protected static void SetHappyPath(InputBuilder arrange)
{
  var dataEntities = arrange.Instance<List<DataEntity>>();
  dataEntities.Add(new() { Id = Guid.NewGuid(), Value = 10, });

  var dataStore = arrange.Instance<IDataStore>();
  dataStore
    .LoadAll()
    .Returns(_ => dataEntities.Select(entity => entity.Clone()));

  arrange
    .Instance<IDataStoreFactory>()
    .GetOrCreateStore(default, default)
    .ReturnsForAnyArgs(dataStore);
}
```

See the SutFactory.Example project for more elaborate examples of using the SUT Factory for advanced cases as well as creating test spies, fakes etc.

The SUT factory uses 4 strategies when creating instances:
* Using an instance provided by an external service provider.
* Using a constructor.
* A partial instance created by NSubstitute.
* A substitute instance created by NSubstitute.

The automatic selection of a strategy should be sufficient for most tests. However, the SutBuilder does allow manual selection of a specific strategy via the "Advanced" input builder. If greater customization is needed, it is possible to replace a strategy by providing a custom implementation through an external service provider when creating the SutBuilder instance. The strategy selection will always prefer creating configurable substitutes unless an explicit interface/implementation pair is registered. In this case, the implementation will be used where the interface is injected into constructors.

Any instance can be retrieved for configuration and inspection. Both the SutBuilder and InputBuilder implement the IServiceProvider interface, allowing the use of the usual GetService and GetRequiredService methods.

A note on avoiding flaky tests. Ensure no Singleton instances is shared by multiple tests when using an external service provider. All instances created by the SUT factory is stored in the SutBuilder instance and is therefore local to the test. Sharing a SutBuilder instance in e.g. SetUp methods is discouraged.
