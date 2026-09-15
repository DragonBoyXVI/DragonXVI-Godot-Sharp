using System.Collections.Generic;
using Godot;

namespace DragonXVI.XVIGodot.StateMachining;


/// <summary>
/// The master node in a node based state machine.
/// 
/// Please note that states are only added at ready time, and adding them at any other time
/// isnt recommended.
/// </summary>
[GlobalClass, Tool, Icon("res://DragonXVI-Godot-Sharp/StateMachining/sm_gear.tres")]
public partial class StateMachineNode : Node{
    /// <summary>
    /// Emitted after this enters a state.
    /// </summary>
    [Signal]
    public delegate void StateEnteredEventHandler( StateNode state );
    /// <summary>
    /// Emitted after this exits a state.
    /// </summary>
    [Signal]
    public delegate void StateExitedEventHandler( StateNode state );
    
    private readonly Dictionary<StringName, StateNode> StateCache = [];
    
    [Export]
    private StateNode? InitialState
    {
        set
        {
            initialState = value;
            UpdateConfigurationWarnings();
        }
        get => initialState;
    }
    private StateNode? initialState;
    /// <summary>
    /// Current state of the state machine.
    /// Use the change method to enter a new state.
    /// </summary>
    public StateNode? CurrentState { get; protected set; }
    
    public override string[] _GetConfigurationWarnings()
    {
        List<string> warnings = [];
        
        if ( InitialState is null )
        {
            warnings.Add( "No initial state is set! Unless you plan to change the state manually at runtime, this machine will do nothing." );
        }
        
        return [.. warnings];
    }
    public override void _Ready()
    {
        if ( Engine.IsEditorHint() )
        {
            XVIFuncs.SetNodeProcesses( this, false );
            return;
        }
        
        // set up children states
        
        // change to initial state if one is set
    }
    
    /// <summary>
    /// Private helper for adding states to this at ready time.
    /// As such, this assumes that the state is already a child of this node.
    /// </summary>
    /// <param name="state"></param>
    private void RegisterState( StateNode state )
    {
        StringName stateName = state.Name;
        
        if ( StateCache.ContainsKey( stateName ) )
        {
            GD.PushError( $"Trying to add dupe state: {stateName}" );
            return;
        }
        
        StateCache[ stateName ] = state;
        state._Disable();
        state.StateChangeRequested += OnStateChangeRequest;
    }
    /// <summary>
    /// Changes the state of this machine.
    /// Depending on what the states are doing, you may want to CallDeferred this.
    /// </summary>
    /// <param name="stateName">Name of the state to change to.</param>
    public void ChangeState( StringName stateName )
    {
        if ( !StateCache.TryGetValue( stateName, out StateNode? state ) )
        {
            GD.PushError( $"Trying to get invalid state: {stateName}" );
            return;
        }
        
        if ( CurrentState is not null )
        {
            if ( !CurrentState.CanSwitchState( state ) )
            {
                return;
            }
            
            CurrentState._ExitState();
            CurrentState._Disable();
            EmitSignal( SignalName.StateExited, CurrentState );
        }
        
        CurrentState = state;
        CurrentState._Enable();
        CurrentState._EnterState();
        EmitSignal( SignalName.StateEntered, CurrentState );
    }
    
    private void OnStateChangeRequest( StringName stateName )
    {
        CallDeferred( MethodName.ChangeState, stateName );
    }
}