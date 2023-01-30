using Hj.SutFactory.Factories;

namespace Hj.SutFactory;

public interface IInstanceFactory
{
  ICtorInstanceFactory CtorInstanceFactory { get; }

  IPartialInstanceFactory PartialInstanceFactory { get; }

  ISubstituteFactory SubstituteFactory { get; }

  object AutoCreate(Type type);
}
