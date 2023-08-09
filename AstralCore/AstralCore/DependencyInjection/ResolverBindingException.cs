using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AstralCore.DependencyInjection;

/// <summary>
/// An exception that is thrown during the binding of <see cref="ITargetResolver"/>s in a <see cref="IServiceLocator"/>.
/// </summary>
public class ResolverBindingException : Exception {
    /// <inheritdoc/>
    public ResolverBindingException() : base() { }

    /// <inheritdoc/>
    public ResolverBindingException(string? message) : base(message) { }

    /// <inheritdoc/>
    public ResolverBindingException(string? message, Exception? innerException) : base(message, innerException) { }

    /// <inheritdoc/>
    public ResolverBindingException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
