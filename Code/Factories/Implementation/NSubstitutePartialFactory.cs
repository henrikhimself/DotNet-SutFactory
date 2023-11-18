using NSubstitute.Core;

namespace Hj.SutFactory.Factories.Implementation;

public sealed class NSubstitutePartialFactory(IServiceProvider serviceProvider) : FactoryBase, IPartialInstanceFactory
{
  private readonly IServiceProvider _serviceProvider = serviceProvider;
  private readonly NSubstitute.Core.ISubstituteFactory _nSubstituteFactory = SubstitutionContext.Current.SubstituteFactory;

  public object Create(Type type)
  {
    var ctor = GetConstructorInfo(type);
    if (ctor == null)
    {
      throw new InvalidOperationException($"Cannot create parts of instance '{type.AssemblyQualifiedName}' due to missing or ambiguous constructors.");
    }

    var parameters = GetConstructorParameters(ctor, _serviceProvider);

    var instance = _nSubstituteFactory.CreatePartial(new Type[] { type }, parameters!);
    return instance;
  }
}
