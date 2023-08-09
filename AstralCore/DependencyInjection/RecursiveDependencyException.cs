using System.Runtime.Serialization;

namespace AstralCore.DependencyInjection;

/// <summary>
/// An exception thrown when services have a recursive dependency on each other.
/// </summary>
public class RecursiveDependencyException : Exception {
    /// <inheritdoc/>
    public RecursiveDependencyException() : base() { }

    /// <inheritdoc/>
    public RecursiveDependencyException(string? message) : base(message) { }

    /// <inheritdoc/>
    public RecursiveDependencyException(string? message, Exception? innerException) : base(message, innerException) { }

    /// <inheritdoc/>
    public RecursiveDependencyException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
