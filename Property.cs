namespace DragonXVI.XVIGodot;

/// <summary>
/// Helper class for Godot property dictionaries.
/// </summary>
public static class Property
{
    /// <summary>
    /// Name of the property, as used in code.
    /// </summary>
    public const string Name = "name";
    /// <summary>
    /// String name of the BUILT IN class. Only used if the property is of TYPE_OBJECT.
    /// </summary>
    public const string ClassName = "class_name";
    /// <summary>
    /// The Variant.Type of this property.
    /// </summary>
    public const string Type = "type";
    /// <summary>
    /// Determines how the editor displays and edits this property.
    /// </summary>
    public const string Hint = "hint";
    /// <summary>
    /// Used with the hint.
    /// </summary>
    public const string HintString = "hint_string";
    /// <summary>
    /// How this property is used.
    /// Example: Is it exported? Is it stored in a save file?
    /// </summary>
    public const string Usage = "usage";
}