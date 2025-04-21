using Hj.SutFactory.TestCase.Case3;

namespace Hj.SutFactory.Benchmarking;

public class Case3Benchmark
{
  [Benchmark]
  public object Case3_Instance_GivenImplementationsOfInterface()
  {
    var sutBuilder = new SutBuilder();

    var inputBuilder = sutBuilder.InputBuilder;
    inputBuilder.Instance<ImplementationInput1>();
    inputBuilder.Instance<ImplementationInput2>();

    return sutBuilder.CreateSut<Case3Sut>();
  }
}
