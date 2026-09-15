using System.Linq;
using Godot;
using Godot.Collections;

namespace DragonXVI.XVIGodot;

/// <summary>
/// A static body with some properties disabled, so they can be enabled in code instead.
/// 
/// Note that all disabled properties are set to 0.
/// </summary>
[GlobalClass, Tool]
public abstract partial class StrippedStaticBody2D : StaticBody2D, IStrippedNode<StrippedStaticBody2D>
{
    public static string[] GetDisabledPropertyNames()
    {
        return [
        CollisionObject2D.PropertyName.CollisionLayer,
        CollisionObject2D.PropertyName.CollisionMask,
        CollisionObject2D.PropertyName.InputPickable,
        CanvasItem.PropertyName.ZIndex,
        ];
    }
    
    public StrippedStaticBody2D()
    {
        CollisionLayer = 0;
        CollisionMask = 0;
        InputPickable = false;
    }
    
    public override void _ValidateProperty(Dictionary property)
    {
        var disabled = GetDisabledPropertyNames();
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

