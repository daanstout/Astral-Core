namespace AstralCore.DependencyInjection;

/// <summary>
/// A locator used to obtain services and inject objects.
/// </summary>
public interface IServiceLocator {
    /// <summary>
    /// Obtains the <see cref="ITargetResolver"/> for a specific type and identifier combination;
    /// </summary>
    /// <param name="type">The type to get the resolver for.</param>
    /// <param name="identifier">The identifier for the specific instance.</param>
    /// <returns>The <see cref="ITargetResolver"/> for the provided type.</returns>
    ITargetResolver Bind(Type type, string? identifier = null);

    /// <summary>
    /// Obtains the <see cref="ITargetResolver"/> for a specific type and identifier combination;
    /// </summary>
    /// <typeparam name="T">The type to get the resolver for.</typeparam>
    /// <param name="identifier">The identifier for the specific instance.</param>
    /// <returns>The <see cref="ITargetResolver"/> for the provided type.</returns>
    ITargetResolver<T> Bind<T>(string? identifier = null) where T : class;

    /// <summary>
    /// Obtains the instance of the requested type with the provided identifier.
    /// </summary>
    /// <param name="type">The type to obtain the instance from.</param>
    /// <param name="identifier">The identifier of the instance to obtain.</param>
    /// <returns>The requested instance.</returns>
    object Get(Type type, string? identifier = null);

    /// <summary>
    /// Obtains the instance of the requested type with the provided identifier.
    /// </summary>
    /// <typeparam name="T">The type to obtain the instance from.</typeparam>
    /// <param name="identifier">The identifier of the instance to obtain.</param>
    /// <returns>The requested instance.</returns>
    T Get<T>(string? identifier = null) where T : class;

    /// <summary>
    /// Injects an object with dependencies.
    /// </summary>
    /// <param name="obj">The object to inject.</param>
    /// <param name="dependencies">The dependencies to inject can be optionally provided.</param>
    void Inject<T>(T obj, object[]? dependencies = null) where T : class;
}
