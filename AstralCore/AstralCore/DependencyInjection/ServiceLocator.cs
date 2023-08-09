using System.Reflection;

namespace AstralCore.DependencyInjection;

/// <summary>
/// A service locator to locate and obtain services.
/// </summary>
public class ServiceLocator : IServiceLocator {
    private readonly record struct ResolverKey(Type Type, string? Identifier);

    private readonly Dictionary<ResolverKey, ITargetResolver> resolvers = new();

    /// <summary>
    /// Constructs a new <see cref="ServiceLocator"/>.
    /// </summary>
    public ServiceLocator() {
        Bind<IServiceLocator>().ToInstance(this);
        Bind<ServiceLocator>().ToInstance(this);
    }

    /// <inheritdoc/>
    public ITargetResolver Bind(Type type, string? identifier = null) => EnsureResolver(type, identifier);

    /// <inheritdoc/>
    public ITargetResolver<T> Bind<T>(string? identifier = null) where T : class => (ITargetResolver<T>)Bind(typeof(T), identifier);

    /// <inheritdoc/>
    public object Get(Type type, string? identifier = null) => Bind(type, identifier).Resolve(this);

    /// <inheritdoc/>
    public T Get<T>(string? identifier = null) where T : class => Bind<T>(identifier).Resolve(this);

    /// <inheritdoc/>
    public void Inject<T>(T obj, object[]? dependencies = null) where T : class {
        var fieldsToInject = Reflect.GetFieldsWithAttribute<T, DependencyAttribute>();

        SetDependencies(obj, fieldsToInject, dependencies ?? ObtainDependencies(fieldsToInject, this));
    }

    private ITargetResolver EnsureResolver(Type type, string? identifier = null) {
        var key = new ResolverKey(type, identifier);

        if (!resolvers.TryGetValue(key, out var resolver)) {
            if (type.IsInterface) {
                var implementations = Reflect.GetInterfaceImplementingTypes(type);

                if (implementations.Length == 0)
                    throw new ResolverBindingException($"No implementations found for interface {type.FullName}");

                if (implementations.Length > 1)
                    Console.WriteLine($"Found {implementations.Length} implementations for interface {type.FullName}, will use first one: {implementations[0].FullName}");

                resolver = CreateResolver(implementations[0]);

                resolvers[key] = resolver;
                resolvers[new ResolverKey(implementations[0], identifier)] = resolver;
            } else {
                resolver = CreateResolver(type);
                resolvers[key] = resolver;
            }
        }

        return resolver;
    }

    private static ITargetResolver CreateResolver(Type type) {
        var resolverType = typeof(TargetResolver<>).MakeGenericType(type);
        return (Activator.CreateInstance(resolverType) as ITargetResolver) ?? throw new Exception($"Failed creating resolver for type {type.FullName}");
    }

    private static object[] ObtainDependencies(FieldInfo[] fields, IServiceLocator serviceLocator) {
        object[] result = new object[fields.Length];

        for (int i = 0; i < fields.Length; i++) {
            result[i] = serviceLocator.Get(fields[i].FieldType);
        }

        return result;
    }

    private static void SetDependencies(object instance, FieldInfo[] fields, object[] objects) {
        for (int i = 0; i < fields.Length; i++) {
            fields[i].SetValue(instance, objects[i]);
        }
    }
}
