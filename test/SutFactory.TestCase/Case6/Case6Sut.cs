namespace Hj.SutFactory.TestCase.Case6;

public class Case6Sut(ExternalService externalService)
{
  public ExternalService ExternalService { get; } = externalService;
}
