using System.Linq;
using Godot;
using Godot.Collections;

namespace DragonXVI.XVIGodot;


/// <summary>
/// A rigid body with some properties disabled, so they can be enabled in code instead.
/// 
/// Note that all disabled properties are set to 0.
/// </summary>
[Tool, GlobalClass]
public partial class StrippedRigidBody2d : RigidBody2D, IStrippedNode<StrippedRigidBody2d>
{
    public static string[] GetDisabledPropertyNames()
    {
        return [
            PropertyName.CollisionLayer,
            PropertyName.CollisionMask,
            PropertyName.CollisionPriority,
            PropertyName.InputPickable,
        ];
    }
    
    public StrippedRigidBody2d()
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