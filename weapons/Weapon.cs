using Godot;

namespace Erebus.Weapons;

abstract public partial class Weapon : Node2D
{
    public Node2D LeftHand;
    public Node2D RightHand;

    public override void _Ready()
    {
        base._Ready();

        LeftHand = GetNode<Node2D>("%LeftHand");
        RightHand = GetNode<Node2D>("%RightHand");
    }

    public void Move(Vector2 dir)
    {
        Rotation = dir.Angle();
    }
}