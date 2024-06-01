namespace Erebus;

using Godot;
using System;

abstract public partial class StateMachine : Node
{
    private int _previousState = -1;
    protected int _state = -1;

    //[Signal]
    //public delegate void StateChangedEventHandler(int newState);
    public void SetState(int newState)
    {
        ExitState(_state);
        _previousState = _state;
        _state = newState;
        // EmitSignal(SignalName.StateChanged, _state);
        EnterState(_previousState, _state);
    }

    public void Update(double delta)
    {
        if (_state != -1)
        {
            StateLogic(delta);
            int transition = GetTransition();
            if (transition != -1)
            {
                SetState(transition);
            }
        }
    }

    abstract protected void StateLogic(double delta);

    abstract protected int GetTransition();

    abstract protected void EnterState(int previousState, int newState);

    abstract protected void ExitState(int stateExited);
}
