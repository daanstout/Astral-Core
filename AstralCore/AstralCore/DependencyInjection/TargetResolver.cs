using System.Reflection;

namespace AstralCore.DependencyInjection;

/// <summary>
/// Stores and resolves objects.
/// </summary>
/// <typeparam name="T"></typeparam>
public class TargetResolver<T> : ITargetResolver<T> where T : class {
    /// <inheritdoc/>
    public bool IsSingleton { get; set; } = true;

    /// <inheritdoc/>
    public IServiceFactory<T>? Factory { get; set; } = null;

    /// <inheritdoc/>
    public ITargetResolver? SubResolver { get; set; } = null;

    private T? instance;
    private bool isResolving;
    private bool isResolved;

    /// <inheritdoc/>
    public T Resolve(IServiceLocator serviceLocator) {
        if (IsSingleton && isResolved)
            return this.instance!;

        if (isResolving)
            throw new RecursiveDependencyException($"Circular dependency found! Already resolving for instance of type {typeof(T)}");

        isResolving = true;

        T? instance;
        object[]? dependencies = null;

        if (SubResolver != null) {
            instance = (T)SubResolver.Resolve(serviceLocator);

            if (IsSingleton) {
                this.instance = instance;
                isResolved = true;
            }

            isResolving = false;
            return instance;
        } else if (Factory != null) {
            instance = Factory.CreateInstance();
            dependencies = Factory.CreateDependencies(serviceLocator);
        } else {
            instance = Activator.CreateInstance<T>();
        }

        serviceLocator.Inject(instance, dependencies);

        if (IsSingleton) {
            isResolved = true;
            this.instance = instance;
        }

        isResolving = false;

        return instance;
    }

    /// <inheritdoc/>
    public void ToInstance(T instance) {
        Reset();

        this.instance = instance;
        isResolved = true;
        isResolving = false;
        IsSingleton = true;
    }

    /// <inheritdoc/>
    public void ToInstance(object instance) => ToInstance((T)instance);

    /// <inheritdoc/>
    object ITargetResolver.Resolve(IServiceLocator serviceLocator) => throw new NotImplementedException();

    

    private void Reset() {
        IsSingleton = true;
        instance = null;
        isResolving = false;
        isResolved = false;
    }
}
