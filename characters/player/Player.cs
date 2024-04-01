using Godot;
using System;

public partial class Player : Character
{
    private Sprite2D sprite;

    public override void _Ready()
    {
        base._Ready();

        sprite = GetNode<Sprite2D>("Sprite2D");
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);

        movDirection.X = Input.GetAxis("move_left", "move_right");
        movDirection.Y = Input.GetAxis("move_up", "move_down");
    }
}
