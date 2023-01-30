namespace Hj.SutFactory.Factories;

public class InstanceFactory : IInstanceFactory
{
  public InstanceFactory(
      ICtorInstanceFactory ctorInstanceFactory,
      IPartialInstanceFactory partialInstanceFactory,
      ISubstituteFactory substituteFactory)
  {
    CtorInstanceFactory = ctorInstanceFactory;
    PartialInstanceFactory = partialInstanceFactory;
    SubstituteFactory = substituteFactory;
  }

  public ICtorInstanceFactory CtorInstanceFactory { get; private set; }

  public IPartialInstanceFactory PartialInstanceFactory { get; private set; }

  public ISubstituteFactory SubstituteFactory { get; private set; }

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
