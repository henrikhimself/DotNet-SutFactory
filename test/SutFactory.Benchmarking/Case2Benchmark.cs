using Hj.SutFactory.TestCase.Case2;

namespace Hj.SutFactory.Benchmarking;

public class Case2Benchmark
{
  [Benchmark]
  public object Case2_Instance_GivenFactory()
  {
    var sutBuilder = new SutBuilder();

    var inputBuilderAdvanced = sutBuilder.InputBuilder.Advanced;
    var implementationInput = inputBuilderAdvanced.Instance(() => new ImplementationInput());

    return sutBuilder.CreateSut<Case2Sut>();
  }

  [Benchmark]
  public object Case2_SubstitutePartsOf_GivenImplementation()
  {
    var sutBuilder = new SutBuilder();

    var inputBuilderAdvanced = sutBuilder.InputBuilder.Advanced;
    inputBuilderAdvanced.SubstitutePartsOf<ImplementationInput>();

    return sutBuilder.CreateSut<Case2Sut>();
  }

  [Benchmark]
  public object Case2_Substitute_GivenInterface()
  {
    var sutBuilder = new SutBuilder();

    var inputBuilderAdvanced = sutBuilder.InputBuilder.Advanced;
    inputBuilderAdvanced.Substitute<IInterfaceInput>();

    return sutBuilder.CreateSut<Case2Sut>();
  }
}
