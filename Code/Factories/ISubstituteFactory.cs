namespace Hj.SutFactory.Factories;

public interface ISubstituteFactory
{
  object Create(params Type[] typeToSubstitute);
}
