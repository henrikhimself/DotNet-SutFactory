using Hj.SutFactory.TestCase.Case1;

namespace Hj.SutFactory.Benchmarking;

public class Case1Benchmark
{
  [Benchmark]
  public object Case1_GivenUnknownRegistrations()
  {
    var sutBuilder = new SutBuilder();
    return sutBuilder.CreateSut<Case1Sut>();
  }

  [Benchmark]
  public object Case1_GivenKnownRegistrations()
  {
    var sutBuilder = new SutBuilder();

    var inputBuilder = sutBuilder.InputBuilder;
    inputBuilder.Instance<IInterfaceInput>();
    inputBuilder.Instance<ClassAbstractInput>();
    inputBuilder.Instance<List<string>>();
    inputBuilder.Instance<ClassGenericInput<List<string>>>();
    inputBuilder.Instance<ClassInput>();
    inputBuilder.Instance<ClassNestedClassInput.ClassInput>();
    inputBuilder.Instance<ClassSealedInput>();
    inputBuilder.Instance<ClassWithDependency>();

    return sutBuilder.CreateSut<Case1Sut>();
  }
}
