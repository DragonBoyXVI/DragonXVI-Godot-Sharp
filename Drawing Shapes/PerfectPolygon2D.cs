using System.Collections.Generic;
using Godot;

namespace DragonXVI.XVIGodot.Drawings;

/// <summary>
/// <para>Draws a multipoint polygon, like pentagons.</para>
/// <para>
/// Does nor provide any of the functionality of actual polygon nodes.
/// Add enough points and this is basically just a circle lol.
/// </para>
/// </summary>
[GlobalClass, Tool]
public partial class PerfectPolygon2D : CircleDrawing2D {
    /// <summary>
    /// How many points the polygon has.
    /// </summary>
    [Export]
    protected int Points {
        private set {
            points = int.Max( 3, value );
            QueueRedraw();
        }
        get => points;
    }
    private int points = 5;

    public override void _Draw()
    {
        if ( Flags == DrawFlags.None ) return;

        List<Vector2> PointArray = [];
        for (int i = 0; i < Points; i++)
        {
            float angle = ( (float)i / Points ) * float.Tau;
            Vector2 vector = Vector2.FromAngle( angle );
            vector *= Radius;
            PointArray.Add( vector + Offset );
        }

        bool antialiasing = Flags.HasFlag( DrawFlags.Antialiasing );
        if ( Flags.HasFlag( DrawFlags.Center ) ) {
            DrawColoredPolygon([.. PointArray], CenterColor );
        }
        if ( Flags.HasFlag( DrawFlags.Outline ) ) {
            PointArray.Add( PointArray[ 0 ] );
            DrawPolyline([.. PointArray], OutlineColor, OutlineThickness, antialiasing );
        }
    }
}