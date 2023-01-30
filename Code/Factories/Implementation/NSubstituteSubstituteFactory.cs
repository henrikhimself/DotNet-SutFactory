using Hj.SutFactory.Factories;
using NSubstitute.Core;

namespace Hj.SutFactory.Factories.Implementation;

public sealed class NSubstituteSubstituteFactory : ISubstituteFactory
{
  private readonly NSubstitute.Core.ISubstituteFactory _nSubstituteFactory;

  public NSubstituteSubstituteFactory() => _nSubstituteFactory = SubstitutionContext.Current.SubstituteFactory;

  public object Create(params Type[] typeToSubstitute)
  {
    var instance = _nSubstituteFactory.Create(typeToSubstitute, Array.Empty<object>());
    return instance;
  }
}
