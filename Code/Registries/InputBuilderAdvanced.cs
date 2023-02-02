namespace Hj.SutFactory.Registries;

public sealed class InputBuilderAdvanced
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IInstanceFactory _instanceFactory;
    private readonly IInputRegistry _inputRegistry;

    public InputBuilderAdvanced(IServiceProvider serviceProvider, IInstanceFactory instanceFactory, IInputRegistry inputRegistry)
    {
        _serviceProvider = serviceProvider;
        _instanceFactory = instanceFactory;
        _inputRegistry = inputRegistry;
    }

    public InputBuilderConfigurator<T> Instance<T>()
        where T : class => Instance<T, T>();

    public InputBuilderConfigurator<TInterface> Instance<TInterface, TImplementation>()
        where TInterface : class
        where TImplementation : class, TInterface
        => GetOrCreateValue<TInterface>(instanceFactory => instanceFactory.CtorInstanceFactory.Create(typeof(TImplementation)));

    public InputBuilderConfigurator<T> SubstitutePartsOf<T>()
        where T : class
        => GetOrCreateValue<T>(instanceFactory => instanceFactory.PartialInstanceFactory.Create(typeof(T)));

    public InputBuilderConfigurator<T> Substitute<T>()
        where T : class
        => GetOrCreateValue<T>(instanceFactory => instanceFactory.SubstituteFactory.Create(typeof(T)));

    private InputBuilderConfigurator<T> GetOrCreateValue<T>(Func<IInstanceFactory, object> valueFactory)
        where T : class
    {
        var value = (T?)_inputRegistry.GetOrCreateValue(
            typeof(T),
            true,
            () => valueFactory(_instanceFactory));
        return new InputBuilderConfigurator<T>(_serviceProvider, value);
    }
}