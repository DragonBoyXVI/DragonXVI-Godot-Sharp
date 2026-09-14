using Godot;

namespace DragonXVI.XVIGodot.Timers;

/// <summary>
/// Supply this with a min and max time and this will pick a random time between
/// the two when started.
/// If supplied with a [RandomNumberGenerator], this will use that.
/// </summary>
[Tool, GlobalClass]
public partial class RandomRangeTimer : XVITimer
{
    /// <summary>
    /// The longest amount of time this can wait for.
    /// </summary>
    [Export(PropertyHint.Range, "0.001,4096.0,0.001,or_greater,exp")]
    public float MaxRandomTime {
        set => maxRandomTime = float.Max(MinRandomTime, value);
        get => maxRandomTime;
    }
    private float maxRandomTime = 2f;
    /// <summary>
    /// The shortest amount of time this can wait for.
    /// </summary>
    [Export(PropertyHint.Range, "0.001,4096.0,0.001,or_greater,exp")]
    public float MinRandomTime
    {
        set
        {
            minRandomTime = float.Min(MaxRandomTime, value);
            if ( Engine.IsEditorHint() ) {
                WaitTime = value;
            }
        }
        get => minRandomTime;
    }
    private float minRandomTime = 1f;

    public override void StartExt()
    {
        if ( Rng is not null ) {
            Start(Rng.RandfRange(minRandomTime, maxRandomTime));
        } else {
            Start(GD.RandRange(minRandomTime, maxRandomTime));
        }
    }
}