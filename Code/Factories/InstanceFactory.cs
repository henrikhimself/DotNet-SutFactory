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

    if (InstanceHelper.IsImplementation(type))
    {
      if (type.IsSealed)
      {
        instance = CtorInstanceFactory.Create(type);
      }
      else
      {
        instance = PartialInstanceFactory.Create(type);
      }
    }
    else
    {
      instance = SubstituteFactory.Create(type);
    }

    return instance;
  }
}
