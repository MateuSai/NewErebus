namespace Erebus.Characters.Player;

using Godot;
using System;

public partial class Player : Character
{
    public Vector2 MouseDirection = Vector2.Zero;

    public enum State
    {
        Idle,
        Move,
    }
    private PlayerStateMachine _stateMachine;

    private Sprite2D _sprite;
    private AnimationPlayer _animationPlayer;
    private Node2D _weapons;

    public override void _Ready()
    {
        base._Ready();

        _sprite = GetNode<Sprite2D>("Sprite2D");
        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        _weapons = GetNode<Node2D>("Weapons");

        _stateMachine = new(this, _animationPlayer);
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);

        MovDirection.X = Input.GetAxis("move_left", "move_right");
        MovDirection.Y = Input.GetAxis("move_up", "move_down");
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (MouseDirection.X > 0 && _sprite.FlipH)
        {
            _sprite.FlipH = false;
        }
        else if (MouseDirection.X < 0 && !_sprite.FlipH)
        {
            _sprite.FlipH = true;
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        MouseDirection = (_weapons.GetLocalMousePosition() + new Vector2(-5, -1)).Normalized();
        _stateMachine.Update(delta);

        base._PhysicsProcess(delta);
    }
}
