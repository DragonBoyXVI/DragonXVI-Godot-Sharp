using Godot;

namespace DragonXVI.XVIGodot.Drawings;

/// <summary>
/// Draws a circle.
/// </summary>
[GlobalClass, Tool]
public partial class CircleDrawing2D : Drawing2D {
    /// <summary>
    /// Radius of the circle, not counting outline thickness.
    /// </summary>
    [Export]
    protected float Radius {
        private set {
            radius = float.Max( 1f, value );
            QueueRedraw();
        }
        get => radius;
    }
    private float radius = 32f;
    /// <summary>
    /// Offset from the center if the circle.
    /// </summary>
    [Export]
    protected Vector2 Offset {
        private set {
            offset = value;
            QueueRedraw();
        }
        get => offset;
    }
    private Vector2 offset = Vector2.Zero;

    public override void _Draw()
    {
        if ( Flags == DrawFlags.None ) return;

        bool antialiasing = Flags.HasFlag( DrawFlags.Antialiasing );
        if ( Flags.HasFlag( DrawFlags.Center ) ) {
            DrawCircle( Offset, Radius, CenterColor, true, -1.0f, antialiasing );
        }
        if ( Flags.HasFlag( DrawFlags.Outline ) ) {
            DrawCircle( Offset, Radius, OutlineColor, false, OutlineThickness, antialiasing );
        }
    }
}