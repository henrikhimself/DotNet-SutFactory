# A SUT Factory.

Like AutoFixture, this library aims to minimize the arrange phase of your unit test.




This is a System-Under-Test (SUT) factory designed to minimize the use of hard-coded constructors in unit tests. In a sizable codebase containing thousands of unit tests, minimizing code used to construct SUT instances allows easier refactoring.

It combines a service provider with a mocking library. The SUT factory enables selective mocking of only the service instances necessary for a given unit test while automatically generating substitutes for any remaining services that is required to construct the SUT.

Construction of instances prefer concrete instances whenever possible to enable the greatest testing depth possible. The SUT factory employs 4 strategies when resolving a dependency graph:
* Using an instance provided by an external service provider.
* Using the default constructor.
* A partial instance provided by the mocking library.
* A substitute mock provided by the mocking library.

This SUT factory currently supports NSubstitute. It is possible to plug in an alternate mocking library.

The automatic selection of a strategy should be enough for most unit tests. However, the SUT factory do allow manually choosing a specific strategy via the "Advanced" builder.

A fluent API guides the order of configuration and allows access to the service provider for custom instance builders when needed. This opinionated approach can either be adopted or, alternately, the SUT factory can be used as just a "smart" service provider.

All state is contained within the SUT factory instance and is therefore local to the scope of each unit test. This enables parallel unit test execution at the cost of some performance. However, it may still be faster with large test suites.

In this example, the HostResolver is a custom service that depends on an instance of IHttpContextAccessor. In the unit test, an IHttpContextAccessor service is arranged to return a configured instance of DefaultHttpContext, designated as the service type HttpContext. The HostResolver implementation may require many other service instances to be injected, but for this unit test they are not needed, so we just let the SUT factory inject substitutes when creating the SUT.
```
// arrange
var sutBuilder = new SutBuilder();
sutBuilder.InputBuilder
    .Instance<IHttpContextAccessor>()
    .Configure(httpContextAccessor =>
    {
        inputBuilder
            .Instance<HttpContext, DefaultHttpContext>()
            .Configure(httpContext =>
            {
                httpContextAccessor.HttpContext = httpContext;

                var uri = new Uri("https://example.com:8080/");
                httpContext.Request.Scheme = uri.Scheme;
                httpContext.Request.Host = new HostString(uri.Host);
            });
    });

var sut = sutBuilder.CreateSut<HostResolver>();

// act
var result = sut.ComposeHostString(UriParts.Scheme, UriParts.Host, UriParts.Port);

// assert
Assert.Equal("https://example.com:8080", result);
```

This second example shows a more advanced use of the SUT factory to arrange the happy path of a document database service. The save and load methods is configured to mimic the external service by storing entities in a list and returning these entities when the load method is invoked. The executing unit test owns the inputBuilder instance which ensures that the entity list is local to only that unit test.

```
private static void SetHappyPath(InputBuilder inputBuilder)
{
    inputBuilder.Instance<IDynamicDataStoreFactory>().Configure(dynamicDataStoreFactory =>
    {
        inputBuilder.Instance<IDynamicDataStore>().Configure(dynamicDataStore =>
        {
            var entities = new List<DdsEntity>();
            inputBuilder.Advanced.Instance(() => entities);

            dynamicDataStoreFactory.CreateStore(Arg.Any<string>(), Arg.Any<Type>()).Returns(dynamicDataStore);

            dynamicDataStore.LoadAll<DdsEntity>().Returns(entities);

            inputBuilder.Advanced.Instance(() => Identity.NewIdentity()).Configure(identity =>
            {
                dynamicDataStore.Save(Arg.Any<DdsEntity>())
                    .Returns(identity)
                    .AndDoes(x =>
                    {
                        var entity = x.ArgAt<DdsEntity>(0);
                        entities.Add(entity);
                        dynamicDataStore.Load<DdsEntity>(Arg.Is<Identity>(i => i == identity)).Returns(entity);
                    });
            });
        });
    });
}
```



