using Hj.SutFactory.UnitTest.TestData.Case1;
using Hj.SutFactory.UnitTest.TestData.Case2;
using Hj.SutFactory.UnitTest.TestData.Case3;
using Hj.SutFactory.UnitTest.TestData.Case4;
using Hj.SutFactory.UnitTest.TestData.Case5;
using Hj.SutFactory.UnitTest.TestData.Case6;
using Hj.SutFactory.UnitTest.TestData.Case7;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NSubstitute.Extensions;

namespace Hj.SutFactory.UnitTest;

public class SutBuilderTests
{
  [Fact]
  public void CreateSut_GivenCase1Sut_CreatesSut()
  {
    // arrange
    var sutBuilder = new SutBuilder(new ExternalServiceProvider());

    var inputBuilder = sutBuilder.InputBuilder;

    inputBuilder.Instance<IInterfaceInput>().Configure((substitute, serviceProvider) =>
    {
      if (serviceProvider.GetService<ExternalService>() is not null)
      {
        substitute.IsTrue().Returns(true);
      }
    });

    inputBuilder.Instance<ImplementationInput>().Configure(partialImplementation =>
    {
      partialImplementation.Configure().IsTrue().Returns(true);
    });

    // act
    var result = sutBuilder.CreateSut<Case1Sut>();

    // assert
    Assert.NotNull(result.InterfaceInputValue);
    Assert.True(result.InterfaceInputValue.IsTrue());

    Assert.NotNull(result.ImplementationInputValue);
    Assert.True(result.ImplementationInputValue.IsTrue());
  }

  [Fact]
  public void CreateSut_GivenCase2Sut_CreatesSut()
  {
    // arrange
    var sutBuilder = new SutBuilder();

    var inputBuilder = sutBuilder.InputBuilder;
    inputBuilder.Instance<ClassAbstractInput>();
    inputBuilder.Instance<ClassGenericInput<object>>();
    inputBuilder.Instance<ClassInput>();
    inputBuilder.Instance<ClassNestedClassInput.ClassInput>();
    inputBuilder.Instance<ClassSealedInput>();

    // act
    var result = sutBuilder.CreateSut<Case2Sut>();

    // assert
    Assert.NotNull(result.ClassAbstractInputValue);
    Assert.NotNull(result.ClassGenericValue);
    Assert.NotNull(result.ClassInputValue);
    Assert.NotNull(result.ClassNestedClassInput);
    Assert.NotNull(result.ClassSealedInputValue);
  }

  [Fact]
  public void CreateSut_GivenCase3Sut_CreatesSut()
  {
    // arrange
    var sutBuilder = new SutBuilder();

    var inputBuilder = sutBuilder.InputBuilder;
    inputBuilder.Instance<ClassKnownInput>();
    inputBuilder.Instance<ClassWithDependency>();

    // act
    var result = sutBuilder.CreateSut<Case3Sut>();

    // assert
    Assert.NotNull(result.ClassWithDependency);
    Assert.NotNull(result.ClassWithDependency.ClassKnownInput);
    Assert.NotNull(result.ClassWithDependency.ClassUnknownInput);

    Assert.NotNull(result.ClassUnknownInput);
    Assert.NotNull(sutBuilder.ServiceProvider.GetService<ClassUnknownInput>());
  }

  [Fact]
  public void CreateSut_GivenCase4Sut_CreatesSut()
  {
    // arrange
    var sutBuilder = new SutBuilder();

    var inputBuilder = sutBuilder.InputBuilder;
    inputBuilder.Advanced.Instance(() => new ClassAmbiguousCtorInput(new object()));
    inputBuilder.Advanced.Instance<ISecondCtor, ClassAmbiguousCtorInput>(() => new ClassAmbiguousCtorInput(new object(), new object()));

    // act
    var result = sutBuilder.CreateSut<Case4Sut>();

    // assert
    Assert.NotNull(result.ClassAmbiguousCtorInput1);
    Assert.NotNull(result.ClassAmbiguousCtorInput2);
  }

  [Fact]
  public void CreateSut_GivenCase4AmbiguousCtorSut_Throws()
  {
    // arrange
    var sutBuilder = new SutBuilder();

    // act & assert
    Assert.Throws<InvalidOperationException>(sutBuilder.CreateSut<Case4AmbiguousCtorSut>);
  }

  [Fact]
  public void CreateSut_GivenCase5Sut_CreatesSut()
  {
    // arrange
    var sutBuilder = new SutBuilder();

    var inputBuilder = sutBuilder.InputBuilder;
    inputBuilder.Instance<Handler1Input>();
    inputBuilder.Instance<Handler2Input>();

    // act
    var result = sutBuilder.CreateSut<Case5Sut>();

    // assert
    Assert.NotEmpty(result.EnumerableInput);
  }

  [Fact]
  public void CreateSut_GivenCase6Sut_CreatesSut()
  {
    // arrange
    var sutBuilder = new SutBuilder();

    var inputBuilder = sutBuilder.InputBuilder;
    inputBuilder.Advanced.Instance<NullInput>(() => null);

    // act
    var result = sutBuilder.CreateSut<Case6Sut>();

    // assert
    Assert.Null(result.NullInput);
  }

  [Fact]
  public void CreateSut_GivenCase7Sut_CreatesSut()
  {
    // arrange
    var sutBuilder = new SutBuilder();

    var inputBuilder = sutBuilder.InputBuilder;
    inputBuilder.Advanced.Instance<InstanceInput>();
    inputBuilder.Advanced.SubstitutePartsOf<ClassPartialSubstituteInput>();
    inputBuilder.Advanced.Substitute<IInterfaceSubstituteInput>();

    // act
    var result = sutBuilder.CreateSut<Case7Sut>();

    // assert
    Assert.NotNull(result.InstanceInput);
    Assert.NotNull(result.ClassPartialSubstituteInput);
    Assert.NotNull(result.IInterfaceSubstituteInput);
  }
}
