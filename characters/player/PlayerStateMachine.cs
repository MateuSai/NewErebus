using Godot;

namespace Erebus.Characters.Player;


public partial class PlayerStateMachine : StateMachine
{
    public enum State
    {
        Idle,
        Move,
    }

    private enum MoveAnimationState
    {
        Move,
        MoveBackwards,
        MoveUp,
        MoveUpBackwards,
    }
    private MoveAnimationState _moveAnimationState;

    private Player _player;
    private AnimationPlayer _animationPlayer;

    public override void _Ready()
    {
        _player = GetParent<Player>();
        _animationPlayer = _player.GetNode<AnimationPlayer>("AnimationPlayer");

        SetState((int)State.Idle);
    }

    protected override int GetTransition()
    {
        switch (_state)
        {
            case (int)State.Idle:
                if (_player.Velocity.Length() > 10)
                {
                    return (int)State.Move;
                }
                break;
            case (int)State.Move:
                if (_player.Velocity.Length() < 10)
                {
                    return (int)State.Idle;
                }
                break;
        }

        return -1;
    }

    protected override void StateLogic(double delta)
    {
        switch (_state)
        {
            case (int)State.Idle:
                //GD.Print("hi");
                if (_player.MouseDirection.Y > 0)
                {
                    _animationPlayer.Play("idle");
                }
                else if (_player.MouseDirection.Y < 0)
                {
                    _animationPlayer.Play("idle_up");
                }
                break;
            case (int)State.Move:
                //GD.Print("ho");
                if (_player.MouseDirection.Y >= 0)
                {
                    if (((_player.MouseDirection.X > 0 && _player.MovDirection.X < 0) || (_player.MouseDirection.X < 0 && _player.MovDirection.X > 0)) && (_animationPlayer.CurrentAnimation != "move" || _moveAnimationState != MoveAnimationState.MoveBackwards))
                    {
                        _moveAnimationState = MoveAnimationState.MoveBackwards;
                        _animationPlayer.PlayBackwards("move");
                    }

                    else if (((_player.MouseDirection.X >= 0 && _player.MovDirection.X >= 0) || (_player.MouseDirection.X <= 0 && _player.MovDirection.X <= 0)) && (_animationPlayer.CurrentAnimation != "move" || _moveAnimationState != MoveAnimationState.Move))
                    {
                        _moveAnimationState = MoveAnimationState.Move;
                        _animationPlayer.Play("move");
                    }
                }
                else
                {
                    if (((_player.MouseDirection.X > 0 && _player.MovDirection.X < 0) || (_player.MouseDirection.X < 0 && _player.MovDirection.X > 0)) && (_animationPlayer.CurrentAnimation != "move_up" || _moveAnimationState != MoveAnimationState.MoveUpBackwards))
                    {
                        _moveAnimationState = MoveAnimationState.MoveUpBackwards;
                        _animationPlayer.PlayBackwards("move_up");
                    }
                    else if (((_player.MouseDirection.X >= 0 && _player.MovDirection.X >= 0) || (_player.MouseDirection.X <= 0 && _player.MovDirection.X <= 0)) && (_animationPlayer.CurrentAnimation != "move_up" || _moveAnimationState != MoveAnimationState.MoveUp))
                    {
                        _moveAnimationState = MoveAnimationState.MoveUp;
                        _animationPlayer.Play("move_up");
                    }
                }
                break;

        }
    }

    protected override void EnterState(int previousState, int newState)
    {
    }

    protected override void ExitState(int stateExited)
    {

    }

}