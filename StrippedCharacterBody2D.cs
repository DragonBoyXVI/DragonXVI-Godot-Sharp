using System.Linq;
using Godot;
using Godot.Collections;

namespace DragonXVI.XVIGodot;

/// <summary>
/// <para>A character body with some properties disabled, so they can be enabled in code instead.</para>
/// <para>
/// Note that all disabled properties are set to 0, while others are default.
/// (e.g. MotionMode is Grounded).
/// </para>
/// </summary>
[GlobalClass, Tool]
public abstract partial class StrippedCharacterBody2D : CharacterBody2D, IStrippedNode<StrippedCharacterBody2D>
{
    public static string[] GetDisabledPropertyNames()
    {
        return [
        PropertyName.MotionMode,
        CollisionObject2D.PropertyName.CollisionLayer,
        CollisionObject2D.PropertyName.CollisionMask,
        CollisionObject2D.PropertyName.InputPickable,
        CanvasItem.PropertyName.ZIndex,
        ];
    }
    
    protected StrippedCharacterBody2D()
    {
        CollisionLayer = 0;
        CollisionMask = 0;
    }
    
    public override void _ValidateProperty(Dictionary property)
    {
        string[] disabled = GetDisabledPropertyNames();
        if (disabled.Contains((string)property[Property.Name]))
        {
            property[Property.Usage] = (long)PropertyUsageFlags.None;
        }
    }
    public override void _Ready()
    {
        if (Engine.IsEditorHint())
        {
            XVIFuncs.SetNodeProcesses(this, false);
            return;
        }
    }
}


