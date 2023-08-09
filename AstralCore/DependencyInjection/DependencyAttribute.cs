namespace AstralCore.DependencyInjection;

/// <summary>
/// Marks a field as a dependency that should be injected.
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public class DependencyAttribute : Attribute {
    /// <summary>
    /// The identifier for the dependency, used to select a specific instance.
    /// </summary>
    public string? Identifier { get; } = null;

    /// <summary>
    /// Constructs a new <see cref="DependencyAttribute"/>.
    /// </summary>
    /// <param name="identifier">The identifier for the dependency, used to select a specific instance.</param>
    public DependencyAttribute(string? identifier = null) => Identifier = identifier;
}
