using System;
using System.Linq;
using Godot;
using Godot.Collections;

namespace DragonXVI.XVIGodot.Drawings;

/// <summary>
/// Base class for drawing shape nodes.
/// Provides some data related to how theyre drawn.
/// </summary>
[GlobalClass, Tool]
public abstract partial class Drawing2D : Node2D {
    [Flags]
    public enum DrawFlags {
        None = 0,
        
        Outline = 1<<0,
        Center = 1<<1,
        Antialiasing = 1<<2,
    }
    
    private static readonly string[] OuttlineNames = [
        PropertyName.OutlineThickness,
        PropertyName.OutlineColor,
    ];
    private static readonly string[] CenterNames = [
        PropertyName.CenterColor,
    ];
    
    /// <summary>
    /// How many pixels thick the outline is.
    /// </summary>
    [ExportGroup( "Outline", "Outline" )]
    [Export]
    protected float OutlineThickness {
        private set {
            outlineThickness = value;
            QueueRedraw();
        }
        get => outlineThickness;
    }
    private float outlineThickness = 3f;
    /// <summary>
    /// Color of the outline.
    /// </summary>
    [Export]
    protected Color OutlineColor {
        private set {
            outlineColor = value;
            QueueRedraw();
        }
        get => outlineColor;
    }
    private Color outlineColor = Colors.Black;
    
    /// <summary>
    /// Color of the shape center.
    /// </summary>
    [ExportGroup("Center", "Center")]
    [Export]
    protected Color CenterColor {
        private set {
            centerColor = value;
            QueueRedraw();
        }
        get => centerColor;
    }
    private Color centerColor = Colors.White;
    
    [ExportGroup("")]
    /// <summary>
    /// Flags that dictate what to draw.
    /// </summary>
    [Export(PropertyHint.Flags, "Draw Outline,Draw Center,Antialiasing")]
    public DrawFlags Flags {
        private set {
            flags = value;
            NotifyPropertyListChanged();
            QueueRedraw();
        }
        get => flags;
    }
    private DrawFlags flags = DrawFlags.Outline | DrawFlags.Center;

    public override void _ValidateProperty(Dictionary property)
    {
        string propertyName = (string)property[ Property.Name ];
        
        if ( OuttlineNames.Contains( propertyName ) && !Flags.HasFlag( DrawFlags.Outline ) ) {
            property[ Property.Usage ] = (long)PropertyUsageFlags.None;
        }
        
        if ( CenterNames.Contains( propertyName ) && !Flags.HasFlag( DrawFlags.Center ) ) {
            property[ Property.Usage ] = (long)PropertyUsageFlags.None;
        }
    }
}