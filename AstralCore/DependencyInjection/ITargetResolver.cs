namespace AstralCore.DependencyInjection;

/// <summary>
/// Stores and resolves objects.
/// </summary>
/// <typeparam name="T">The type to resolve.</typeparam>
public interface ITargetResolver<T> : ITargetResolver where T : class {
    /// <summary>
    /// The factory to use when resolving the instance.
    /// </summary>
    IServiceFactory<T>? Factory { get; set; }

    /// <summary>
    /// Sets the target to a specific instance.
    /// </summary>
    /// <param name="instance">The instance that should be resolved to.</param>
    void ToInstance(T instance);

    /// <summary>
    /// Resolves for the target.
    /// </summary>
    /// <param name="serviceLocator">The <see cref="IServiceLocator"/> to obtain the dependencies for.</param>
    /// <returns>The resolved instance.</returns>
    new T Resolve(IServiceLocator serviceLocator);
}

/// <summary>
/// Stores and resolves objects.
/// </summary>
public interface ITargetResolver {
    /// <summary>
    /// If <see langword="true"/>, the same instance will be resolved to. If <see langword="false"/>, a new instance will be created with each resolve.
    /// </summary>
    bool IsSingleton { get; set; }

    /// <summary>
    /// The resolver to use when resolving the instance.
    /// </summary>
    ITargetResolver? SubResolver { get; set; }

    /// <summary>
    /// Sets the target to a specific instance.
    /// </summary>
    /// <param name="instance">The instance that should be resolved to.</param>
    void ToInstance(object instance);

    /// <summary>
    /// Resolves for the target.
    /// </summary>
    /// <param name="serviceLocator">The <see cref="IServiceLocator"/> to obtain the dependencies for.</param>
    /// <returns>The resolved instance.</returns>
    object Resolve(IServiceLocator serviceLocator);
}
