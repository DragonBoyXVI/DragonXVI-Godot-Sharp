using Godot;

namespace DragonXVI.XVIGodot.StateMachining;

/// <summary>
/// Base class for states use by the StateMachineNode.
/// </summary>
[GlobalClass, Tool, Icon("res://DragonXVI-Godot-Sharp/StateMachining/sm_text.tres")]
public abstract partial class StateNode : Node
{

    /// <summary>
    /// Emitted to tell the parent StateMachineNode to change states.
    /// </summary>
    [Signal]
    public delegate void StateChangeRequestedEventHandler( StringName stateName );

    public override void _Ready()
    {
        if ( Engine.IsEditorHint() )
        {
            XVIFuncs.SetNodeProcesses( this, false );
            return;
        }
    }
    
    #pragma warning disable IDE1006
    /// <summary>
    /// Called by the state machine when this state is entered.
    /// </summary>
    public virtual void _EnterState() {}

    /// <summary>
    /// Called by the state machine when leaving this state.
    /// </summary>
    public virtual void _ExitState() {}
    /// <summary>
    /// Tester for if this can swap to a new state.
    /// By default, this stops states from transitioning into themselves.
    /// </summary>
    /// <param name="state">The state this is moving into.</param>
    public virtual bool CanSwitchState( StateNode state ) => Name != state.Name;
    /// <summary>
    /// States are enabled by the state machine when in use.
    /// </summary>
    public virtual void _Enable() {
        ProcessMode = ProcessModeEnum.Inherit;
    }
    /// <summary>
    /// States are disabled by the state machine when not in use.
    /// </summary>
    public virtual void _Disable() {
        ProcessMode = ProcessModeEnum.Disabled;
    }
    #pragma warning restore IDE1006

    /// <summary>
    /// Call this to request that the parent StateMachineNode changes states.
    /// </summary>
    /// <param name="stateName">Name of the state to switch to.</param>
    protected void RequestState( StringName stateName ) => EmitSignal( SignalName.StateChangeRequested, stateName );
}