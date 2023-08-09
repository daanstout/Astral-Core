namespace AstralCore;

/// <summary>
/// A filter to only check specific types of <see cref="Type"/>.
/// </summary>
[Flags]
public enum TypeFilter : uint {
    /// <summary>
    /// No filter should be applied.
    /// </summary>
    None = 0x00,
    /// <summary>
    /// Allow Instantiatable types (Does not include value types).
    /// </summary>
    Instantiatable = 0x01,
    /// <summary>
    /// Allow abstract types.
    /// </summary>
    Abstract = 0x02,
    /// <summary>
    /// Allow interfaces.
    /// </summary>
    Interface = 0x04,
    /// <summary>
    /// Allow value types.
    /// </summary>
    ValueType = 0x08,

    /// <summary>
    /// Default value, only <see cref="Instantiatable"/>
    /// </summary>
    Default = Instantiatable,

    /// <summary>
    /// Allow all types.
    /// </summary>
    All = uint.MaxValue
}
