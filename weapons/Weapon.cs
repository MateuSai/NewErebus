using Godot;

namespace Erebus.Weapons;

abstract public partial class Weapon : Node2D
{
	public enum State
	{
		Idle,
		Move,
		Engage,
	}
	private State _state = State.Idle;

	private Node2D _leftHand;
	private Node2D _rightHand;
	private AnimationPlayer _animationPlayer;

	public override void _Ready()
	{
		base._Ready();

		// This is only for reference at the moment of making the animations
		GetNode<Sprite2D>("PlayerReference").QueueFree();
		GetNode<Sprite2D>("PlayerReferenceUp").QueueFree();

		_leftHand = GetNode<Node2D>("%LeftHand");
		_rightHand = GetNode<Node2D>("%RightHand");
		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");

		//LeftHand.Hide();
		//RightHand.Hide();
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

	public void Attack()
	{
		_animationPlayer.Play("Attack_01");
		_state = State.Engage;
	}

	public void StartIdleAnimation()
	{
		_animationPlayer.Play("Idle");
		_state = State.Idle;
	}

	public void StartMovingAnimation()
	{
		_animationPlayer.Play("Move");
		_state = State.Move;
	}

	public bool IsBusy()
	{
		return _animationPlayer.IsPlaying() && _animationPlayer.CurrentAnimation.StartsWith("Attack");
	}

	public State GetState()
	{
		return _state;
	}
}
