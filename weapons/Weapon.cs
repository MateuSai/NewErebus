using Godot;

namespace Erebus.Weapons;

abstract public partial class Weapon : Node2D
{
    public Node2D LeftHand;
    public Node2D RightHand;

    public override void _Ready()
    {
        base._Ready();

        // This is only for reference at the moment of making the animations
        GetNode<Sprite2D>("PlayerReference").QueueFree();

        LeftHand = GetNode<Node2D>("%LeftHand");
        RightHand = GetNode<Node2D>("%RightHand");
    }

    public void Move(Vector2 dir)
    {
        Rotation = dir.Angle();

        if (Scale.Y == 1 && Vector2.Right.Rotated(Rotation).X < 0)
        {
            Scale = new Vector2(Scale.X, -1);
        }
        else if (Scale.Y == -1 && Vector2.Right.Rotated(Rotation).X > 0)
        {
            Scale = new Vector2(Scale.X, 1);
        }
    }
}