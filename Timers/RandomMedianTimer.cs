using Godot;

namespace DragonXVI.GodotTemplate.Timers;

/// <summary>
/// Uses the randfn function to pick random times along a standard deviation.
/// This makes for more natural feeling randomness, but be mindfull that extreme
/// outliers are an (unlikely) possibility.
/// Use an rng object to control its rng state.
/// </summary>
[Tool, GlobalClass]
public partial class RandomMedianTimer : XVITimer
{
	/// <summary>
	/// The median or "expected" amount of time.
    /// Picked wait time is deviated from here.
	/// </summary>
    [Export(PropertyHint.Range, "0.001,4096.0,0.001,or_greater,exp")]
    public float MeanTime {
		set {
            meanTime = value;
			if (Engine.IsEditorHint()) {
                WaitTime = value * 0.5f;
            }
        }
        get => meanTime;
    }
    private float meanTime = 1f;
	/// <summary>
	/// The standard deviation from the mean.
	/// </summary>
    [Export]
    public float TimeDeviation {
		set {
            timeDeviation = float.Max(0.001f, float.Abs(value));
        }
        get => timeDeviation;
    }
    private float timeDeviation = 0.125f;

    public override void StartExt()
    {
		if ( Rng is not null ) {
            Start(Rng.Randfn(meanTime, timeDeviation));
        } else {
            Start(GD.Randfn(meanTime, timeDeviation));
		}
    }
}