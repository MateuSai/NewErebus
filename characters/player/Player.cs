namespace Erebus.Characters.Player;

using Godot;
using System;

public partial class Player : Character
{
    public Vector2 MousePosition = Vector2.Zero;
    public Vector2 MouseDirection = Vector2.Zero;

    public enum ShouldersDir
    {
        Top,
        Bottom,
    }

    public enum State
    {
        Idle,
        Move,
    }
    private PlayerStateMachine _stateMachine;

    private Sprite2D _sprite;
    private AnimationPlayer _animationPlayer;
    private Node2D _weapons;
    private Bone2D _rightArm;
    private Bone2D _leftArm;

    public override void _Ready()
    {
        base._Ready();

        _sprite = GetNode<Sprite2D>("Sprite2D");
        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        _weapons = GetNode<Node2D>("Weapons");
        _rightArm = GetNode<Bone2D>("Skeleton/Torso/RightArm");
        _leftArm = GetNode<Bone2D>("Skeleton/Torso/LeftArm");

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

        if (MousePosition.X > 1 && Scale.X == -1)
        {
            Scale = new Vector2((float)1.0, Scale.Y);
        }
        else if (MousePosition.X < -1 && Scale.X == 1)
        {
            Scale = new Vector2((float)-1.0, Scale.Y);
        }

        /*  if (MouseDirection.X > 0 && _sprite.FlipH)
         {
             _sprite.FlipH = false;
         }
         else if (MouseDirection.X < 0 && !_sprite.FlipH)
         {
             _sprite.FlipH = true;
         } */
    }

    public override void _PhysicsProcess(double delta)
    {
        MousePosition = _weapons.GetLocalMousePosition();
        //MouseDirection = (_weapons.GetLocalMousePosition() + new Vector2(-5, -1)).Normalized();
        MouseDirection = MousePosition.Normalized();
        //GetNode<Node2D>("IKRightHandTarget").Position = GetLocalMousePosition();
        //GetNode<Node2D>("IKLeftHandTarget").Position = GetLocalMousePosition();
        _stateMachine.Update(delta);

        base._PhysicsProcess(delta);
    }

    public void UpdateShoulderBones(ShouldersDir shouldersDir)
    {
        switch (shouldersDir)
        {
            case ShouldersDir.Bottom:
                _rightArm.Position = new Vector2(-3, 13);
                _leftArm.Position = new Vector2(-3, -8);
                break;
            case ShouldersDir.Top:
                _rightArm.Position = new Vector2(1, -9);
                _leftArm.Position = new Vector2(-4, 9);
                break;
        }
    }
}
