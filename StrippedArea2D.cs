using Godot;
using Godot.Collections;

namespace DragonXVI.XVIGodot;

/// <summary>
/// <para>An Area2D with some properies disabled, so they can be enabled in code instead.</para>
/// <para>NOTE: Disabled properties are set to their OFF state. E.g. Monitoring/able is false, and collision layer/mask is 0.</para>
/// </summary?
[GlobalClass, Tool]
public abstract partial class StrippedArea2D : Area2D, IStrippedNode<StrippedArea2D>
{
    public static string[] GetDisabledPropertyNames()
    {
        return [
            PropertyName.Monitoring,
            PropertyName.Monitorable,
            CollisionObject2D.PropertyName.CollisionLayer,
            CollisionObject2D.PropertyName.CollisionMask,
            CollisionObject2D.PropertyName.InputPickable,
            CanvasItem.PropertyName.ZIndex,
        ];
    }
    
    protected StrippedArea2D() {
        Monitoring = false;
        Monitorable = false;
        CollisionLayer = 0;
        CollisionMask = 0;
        InputPickable = false;
    }

    public override void _Ready()
    {
        if ( Engine.IsEditorHint() ) {
            
            ChildEnteredTree += OnChildEnteredTree;
            
            XVIFuncs.SetNodeProcesses( this, false );
            return;
        }
    }
    public override void _ValidateProperty( Dictionary property ) {
        var disabled = GetDisabledPropertyNames();
        if ( disabled.Contains( (string)property[ Property.Name ] ) ) {
            property[ Property.Usage ] = (long)PropertyUsageFlags.None;
        }
    }

    /// <summary>
    /// Runs in the editor only.
    /// </summary>
    private void OnChildEnteredTree( Node node ) {
        if ( node is CollisionShape2D shape ) {
            ShapeEnteredTree( shape );
        }
    }

    /// <summary>
    /// Shortcut for a collision shape child entering the tree.
    /// I like to use this to change its debug color.
    /// </summary>
    protected virtual void ShapeEnteredTree( CollisionShape2D shape ) {}
}