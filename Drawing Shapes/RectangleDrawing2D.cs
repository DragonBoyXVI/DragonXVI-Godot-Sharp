using Godot;

namespace DragonXVI.XVIGodot.Drawings;

/// <summary>
/// Draws a rectangle to the screen.
/// </summary>
[GlobalClass, Tool]
public partial class RectangleDrawing2D : Drawing2D {
    
    /// <summary>
    /// Size of the rectangle.
    /// </summary>
    [Export]
    protected Vector2 Size {
        private set {
            size = value.Abs();
            QueueRedraw();
        }
        get => size;
    }
    private Vector2 size = new( 32f, 32f );
    /// <summary>
    /// Rectangle offset.
    /// 
    /// If FromCenter is true, this is from the center.
    /// Otherwise its from the top left corner.
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
    /// <summary>
    /// If turned off, the rectangle originates from the top left.
    /// </summary>
    [Export]
    protected bool FromCenter {
        private set {
            fromCenter = value;
            QueueRedraw();
        }
        get => fromCenter;
    }
    private bool fromCenter = true;
    
    public override void _Draw() {
        if ( Flags == DrawFlags.None ) return;
        
        bool antialiasing = Flags.HasFlag( DrawFlags.Antialiasing );
        Rect2 rect = new( Offset, Size );
        if ( FromCenter ) rect.Position -= Size * 0.5f;
        if ( Flags.HasFlag( DrawFlags.Center ) ) {
            DrawRect( rect, CenterColor, true, -1f, antialiasing );
        }
        if ( Flags.HasFlag( DrawFlags.Outline ) ) {
            DrawRect( rect, OutlineColor, false, OutlineThickness, antialiasing );
        }
    }
}