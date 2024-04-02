namespace Erebus.Characters;

using Godot;

public partial class Character : CharacterBody2D
{
    public Vector2 MovDirection = Vector2.Zero;
    protected int maxSpeed = 300;


    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        Velocity = MovDirection.Normalized() * maxSpeed;
        MoveAndSlide();
    }
}
