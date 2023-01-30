using Hj.SutFactory.Factories;

namespace Hj.SutFactory.Factories.Implementation;

public sealed class CtorInstanceFactory : FactoryBase, ICtorInstanceFactory
{
  private readonly IServiceProvider _serviceProvider;

  public CtorInstanceFactory(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

  public object Create(Type type)
  {
    var ctor = GetConstructorInfo(type);
    if (ctor == null)
    {
      throw new InvalidOperationException($"Cannot create instance '{type.AssemblyQualifiedName}' due to missing or ambiguous constructors.");
    }

    var parameters = GetConstructorParameters(ctor, _serviceProvider);

    var instance = ctor.Invoke(parameters);
    return instance;
  }
}
