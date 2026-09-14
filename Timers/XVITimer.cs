using Godot;
using Godot.Collections;

namespace DragonXVI.XVIGodot.Timers;

/// <summary>
/// Base custom timer class.
/// </summary>
[Tool, GlobalClass]
public abstract partial class XVITimer : Godot.Timer, IStrippedNode<XVITimer>
{
    public static string[] GetDisabledPropertyNames()
    {
        return [
            Godot.Timer.PropertyName.WaitTime,
            Godot.Timer.PropertyName.Autostart,
        ];
    }
   
    /// <summary>
    /// Like AutoStart on the base timer, except this one uses the custom start.
    /// </summary>
    [Export]
    public bool StartAutomatically = false;
    
    /// <summary>
    /// If set, this is used for randomness instead of the global randoms.
    /// </summary>
    public RandomNumberGenerator? Rng = null;
    
    public override void _ValidateProperty(Dictionary property)
    {
        string[] disabled = GetDisabledPropertyNames();
        if ( disabled.Contains<string>( property[ Property.Name ].ToString() ) )
        {
            property[ Property.Usage ] = (long)PropertyUsageFlags.None;
        }
    }

    public override void _Ready()
    {
        if ( Engine.IsEditorHint() )
        {
            XVIFuncs.SetNodeProcesses( this, false );
            return;
        }
        
        if ( StartAutomatically )
        {
            StartAutomatically = false;
            // work around for methodname not working without godot generated files.
            Callable.From( StartExt ).CallDeferred();
        }
    }
    
    /// <summary>
    /// Like Start on the base timer, but uses this's custom functionality.
    /// </summary>
    public abstract void StartExt();
    
    
    protected void OnSelfTimeout()
    {
        if ( !OneShot )
        {
            StartExt();
        }
    }
}