namespace Hj.SutFactory.Factories;

public class InstanceFactory(
  ICtorInstanceFactory ctorInstanceFactory,
  IPartialInstanceFactory partialInstanceFactory,
  ISubstituteFactory substituteFactory) : IInstanceFactory
{
  public ICtorInstanceFactory CtorInstanceFactory { get; } = ctorInstanceFactory;

  public IPartialInstanceFactory PartialInstanceFactory { get; } = partialInstanceFactory;

  public ISubstituteFactory SubstituteFactory { get; } = substituteFactory;

  public object AutoCreate(Type type)
  {
    object instance;

    if (type.IsInterface || type.IsAbstract)
    {
      instance = SubstituteFactory.Create(type);
    }
    else if (type.IsSealed)
    {
      instance = CtorInstanceFactory.Create(type);
    }
    else
    {
      instance = PartialInstanceFactory.Create(type);
    }

    return instance;
  }
}
