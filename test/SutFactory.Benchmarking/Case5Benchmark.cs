using Hj.SutFactory.TestCase.Case5;

namespace Hj.SutFactory.Benchmarking;

public class Case5Benchmark
{
  [Benchmark]
  public object Case5_Null()
  {
    var sutBuilder = new SutBuilder();

    var inputBuilder = sutBuilder.InputBuilder;
    inputBuilder.Null<NullInput>();

    return sutBuilder.CreateSut<Case5Sut>();
  }
}
