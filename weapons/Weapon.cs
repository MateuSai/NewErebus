using Godot;

namespace Erebus.Weapons;

abstract public partial class Weapon : Node2D
{
    public Node2D LeftHand;
    public RemoteTransform2D LeftHandRemoteTransform;
    public Node2D RightHand;
    public RemoteTransform2D RightHandRemoteTransform;

    public override void _Ready()
    {
        base._Ready();

        // This is only for reference at the moment of making the animations
        GetNode<Sprite2D>("PlayerReference").QueueFree();
        GetNode<Sprite2D>("PlayerReferenceUp").QueueFree();

        LeftHand = GetNode<Node2D>("%LeftHand");
        LeftHandRemoteTransform = LeftHand.GetNode<RemoteTransform2D>("RemoteTransform2D");
        RightHand = GetNode<Node2D>("%RightHand");
        RightHandRemoteTransform = RightHand.GetNode<RemoteTransform2D>("RemoteTransform2D");

        LeftHand.Hide();
        RightHand.Hide();
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
