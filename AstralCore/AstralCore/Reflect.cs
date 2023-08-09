using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace AstralCore;

/// <summary>
/// A class containing helper Reflection functions.
/// </summary>
public static class Reflect {
    private readonly record struct TypeAttributeInfo(Type Type, Type AttributeType);

    private readonly record struct TypeFilterInfo(Type Type, TypeFilter TypeFilter);

    private static readonly Dictionary<Type, Type[]> interfaceImplementations = new();
    private static readonly HashSet<Type> types = new();
    private static readonly Dictionary<TypeAttributeInfo, FieldInfo[]> typeAttributeCache = new();
    //private static readonly Dictionary<TypeFilterInfo, Type[]> childTypeCache = new();

    static Reflect() {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        foreach (var assembly in assemblies) {
            foreach (var type in assembly.GetTypes()) {
                types.Add(type);
            }
        }
    }

    /// <summary>
    /// Gets all types that implement an interface.
    /// </summary>
    /// <typeparam name="T">The interface to find types for.</typeparam>
    /// <returns>All types that implement <typeparamref name="T"/></returns>
    /// <exception cref="ArgumentException">Thrown if <typeparamref name="T"/> is not an interface.</exception>
    [StackTraceHidden]
    public static Type[] GetInterfaceImplementingTypes<T>() => GetInterfaceImplementingTypes(typeof(T));

    /// <summary>
    /// Gets all types that implement an interface.
    /// </summary>
    /// <param name="type">The interface to find types for.</param>
    /// <returns>All types that implement <paramref name="type"/></returns>
    /// <exception cref="ArgumentException">Thrown if <paramref name="type"/> is not an interface.</exception>
    public static Type[] GetInterfaceImplementingTypes(Type type) {
        if (!type.IsInterface)
            throw new ArgumentException($"Type {type.FullName} is not an interface!");

        if (!interfaceImplementations.TryGetValue(type, out var result)) {
            List<Type> implementations = new();

            foreach (var t in types) {
                if (t.GetInterfaces().Contains(type))
                    implementations.Add(t);
            }

            result = implementations.ToArray();
            interfaceImplementations[type] = result;
        }

        return result;
    }

    /// <summary>
    /// Gets all fields within <typeparamref name="TType"/> that have the <typeparamref name="TAttribute"/> attribute on them.
    /// </summary>
    /// <typeparam name="TType">The type to check the fields for.</typeparam>
    /// <typeparam name="TAttribute">The attribute that the fields need to have.</typeparam>
    /// <returns>All fields within <typeparamref name="TType"/> that have the <typeparamref name="TAttribute"/> attribute.</returns>
    public static FieldInfo[] GetFieldsWithAttribute<TType, TAttribute>() where TAttribute : Attribute => GetFieldsWithAttribute(typeof(TType), typeof(TAttribute));

    /// <summary>
    /// Gets all fields within <paramref name="type"/> that have the <paramref name="attribute"/> attribute on them.
    /// </summary>
    /// <param name="type">The type to check the fields for.</param>
    /// <param name="attribute">The attribute that the fields need to have.</param>
    /// <returns>All fields within <paramref name="type"/> that have the <paramref name="attribute"/> attribute.</returns>
    /// <exception cref="ArgumentException">Thrown if <paramref name="attribute"/> does not inherit from <see cref="Attribute"/>.</exception>
    public static FieldInfo[] GetFieldsWithAttribute(Type type, Type attribute) {
        if (!typeof(Attribute).IsAssignableFrom(attribute))
            throw new ArgumentException($"{attribute.FullName} does not inherit from Attribute!");

        var key = new TypeAttributeInfo(type, attribute);

        if(!typeAttributeCache.TryGetValue(key, out var result)) {
            List<FieldInfo> fields = new();

            while(type != null && !type.IsInterface) {
                fields.AddRange(type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                    .Where(field => field.GetCustomAttribute(attribute) != null));

                type = type.BaseType!;
            }

            result = fields.ToArray();
            typeAttributeCache[key] = result;
        }

        return result;
    }
}
