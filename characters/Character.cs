using Godot;

public partial class Character : CharacterBody2D
{
    protected Vector2 movDirection = Vector2.Zero;
    protected int maxSpeed = 300;


    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        Position += movDirection.Normalized() * maxSpeed * (float)delta;
    }
}