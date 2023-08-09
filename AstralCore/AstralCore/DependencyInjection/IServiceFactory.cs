namespace AstralCore.DependencyInjection;

/// <summary>
/// Allows for creation of
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IServiceFactory<T> {
    /// <summary>
    /// Creates an instance of the object.
    /// </summary>
    /// <returns>The instance this factory is made for.</returns>
    T CreateInstance();

    /// <summary>
    /// Creates the dependencies this object uses.
    /// </summary>
    /// <param name="serviceLocator">The <see cref="IServiceLocator"/> that can be used to obtain the dependencies.</param>
    /// <returns>The dependencies the object this factory creates uses.</returns>
    object[] CreateDependencies(IServiceLocator serviceLocator);
}
