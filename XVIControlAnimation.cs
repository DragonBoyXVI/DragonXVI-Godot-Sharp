using Godot;

namespace DragonXVI.XVIGodot;

/// <summary>
/// Extremely simple "out of the box" style ui animations.
/// Meant to be something quick and dirty to use for prototyping.
/// </summary>
public static class XVIControlAnimation
{
    

    /// <summary>
    /// Metadata name for the tween used for animation.
    /// This tween is stored in the metadata for the provided nodes.
    /// </summary>
    private static readonly StringName MetadataTween = "CurrentTween";

    private static readonly NodePath PathModulate = CanvasItem.PropertyName.Modulate.ToString();
    private static readonly NodePath PathOffsetTransScale = Control.PropertyName.OffsetTransformScale.ToString();
    private static readonly Vector2 ClosedWindowScale = Vector2.One * 0.8f;


    /// <summary>
    /// Util for killing active tweens.
    /// </summary>
    /// <param name="control">The node who owns the tween.</param>
    private static void KillTween(Control control)
    {
        if (!control.HasMeta(MetadataTween))
        {
            return;
        }

        Tween tween = (Tween)(GodotObject)control.GetMeta(MetadataTween);
        if (tween.IsRunning())
        {
            tween.Kill();
        }
    }


    /// <summary>
    /// Plays an animation like that of a closing computer window.
    /// Hides the node once the animation is finished.
    /// </summary>
    /// <param name="control">The node to animate.</param>
    /// <param name="duration">How long the animation lasts, in seconds. Default 0.125 seconds.</param>
    public static Tween CloseWindow(Control control, double duration = 0.125)
    {
        KillTween(control);
        control.OffsetTransformEnabled = true;

        Tween tween = control.CreateTween();
        control.SetMeta(MetadataTween, tween);

        _ = tween.SetIgnoreTimeScale();
        _ = tween.SetPauseMode(Tween.TweenPauseMode.Process);
        _ = tween.SetParallel();

        _ = tween.TweenProperty(control, PathModulate, Colors.Transparent, duration);
        _ = tween.TweenProperty(control, PathOffsetTransScale, ClosedWindowScale, duration);
        _ = tween.TweenCallback(Callable.From(control.Hide)).SetDelay(duration);

        return tween;
    }

    /// <summary>
    /// Plays an animation like that of an opening computer window.
    /// If "stageNode" is true (it is by default) the node will be set to the expected starting state for this animation.
    /// </summary>
    /// <param name="control">The node to animate.</param>
    /// <param name="stageNode">Sets the node to be in a closed state, so the animation can open it. Defaults to true.</param>
    /// <param name="duration">How long the animation lasts, in seconds. Defaults to 0.125 seconds.</param>
    public static void OpenWindow(Control control, bool stageNode = true, double duration = 0.125)
    {
        KillTween(control);

        if (stageNode)
        {
            control.OffsetTransformScale = ClosedWindowScale;
            control.Modulate = Colors.Transparent;
        }

        Tween tween = control.CreateTween();
        control.SetMeta(MetadataTween, tween);

        _ = tween.SetIgnoreTimeScale();
        _ = tween.SetPauseMode(Tween.TweenPauseMode.Process);
        _ = tween.SetParallel();

        _ = tween.TweenProperty(control, PathModulate, Colors.White, duration);
        _ = tween.TweenProperty(control, PathOffsetTransScale, Vector2.One, duration);
        //tween.TweenCallback( Callable.From( control.Show ) );
        control.Show();
    }
}