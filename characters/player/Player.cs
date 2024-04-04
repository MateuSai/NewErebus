namespace Erebus.Characters.Player;

using Erebus.Weapons;
using Godot;
using System;

public partial class Player : Character
{
	public Vector2 MousePosition = Vector2.Zero;
	public Vector2 MouseDirection = Vector2.Zero;

	private enum FacingDir
	{
		TopLeft,
		TopRight,
		BottomLeft,
		BottomRight,
	}
	private FacingDir _facingDir = FacingDir.BottomRight;

	public enum State
	{
		Idle,
		Move,
	}
	private PlayerStateMachine _stateMachine;

	private Sprite2D _sprite;
	private AnimationPlayer _animationPlayer;
	private PlayerWeapons _weapons;
	private Skeleton2D _skeleton2D;
	private SkeletonModificationStack2D _skeletonModifications;
	private SkeletonModification2DTwoBoneIK _rightIK;
	private SkeletonModification2DTwoBoneIK _leftIK;
	private Bone2D _rightArm;
	private Bone2D _leftArm;

	public override void _Ready()
	{
		base._Ready();

		_sprite = GetNode<Sprite2D>("Sprite2D");
		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		_weapons = GetNode<PlayerWeapons>("Weapons");
		_skeleton2D = GetNode<Skeleton2D>("Skeleton");
		_skeletonModifications = _skeleton2D.GetModificationStack();
		_rightIK = ((SkeletonModification2DTwoBoneIK)_skeletonModifications.GetModification(0));
		_leftIK = ((SkeletonModification2DTwoBoneIK)_skeletonModifications.GetModification(1));
		_rightArm = _skeleton2D.GetNode<Bone2D>("Torso/RightArm");
		_leftArm = _skeleton2D.GetNode<Bone2D>("Torso/LeftArm");

		_stateMachine = new(this, _animationPlayer);

		_weapons.Start();
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

		/* if (MousePosition.X > 1 && Scale.X == -1)
		{
			Scale = new Vector2((float)1.0, Scale.Y);
			_rightIK.FlipBendDirection = true;
			_leftIK.FlipBendDirection = true;
		}
		else if (MousePosition.X < -1 && Scale.X == 1)
		{
			Scale = new Vector2((float)-1.0, Scale.Y);
			_rightIK.FlipBendDirection = false;
			_leftIK.FlipBendDirection = false;
		} */

		if (MousePosition.X > 1 && _sprite.FlipH)
		{
			_sprite.FlipH = false;
			_rightIK.FlipBendDirection = true;
			_leftIK.FlipBendDirection = true;
		}
		else if (MousePosition.X < -1 && !_sprite.FlipH)
		{
			_sprite.FlipH = true;
			_rightIK.FlipBendDirection = false;
			_leftIK.FlipBendDirection = false;
		}

		if (MouseDirection.Y >= 0)
		{
			if (MouseDirection.X < 0 && _facingDir != FacingDir.BottomLeft)
			{
				SetFacingDir(FacingDir.BottomLeft);
			}

			else if (MouseDirection.X > 0 && _facingDir != FacingDir.BottomRight)
			{
				SetFacingDir(FacingDir.BottomRight);
			}
		}
		else
		{
			if (MouseDirection.X < 0 && _facingDir != FacingDir.TopLeft)
			{
				SetFacingDir(FacingDir.TopLeft);
			}
			else if (MouseDirection.X > 0 && _facingDir != FacingDir.TopRight)
			{
				SetFacingDir(FacingDir.TopRight);
			}
		}
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

		_weapons.MoveCurrentWeapon(MouseDirection);
	}

	private void UpdateShoulderBones()
	{
		switch (_facingDir)
		{
			case FacingDir.BottomRight:
				_rightArm.Position = new Vector2(-3, 13);
				_leftArm.Position = new Vector2(-3, -8);
				break;
			case FacingDir.TopRight:
				_rightArm.Position = new Vector2(1, -9);
				_leftArm.Position = new Vector2(-4, 9);
				break;
			case FacingDir.BottomLeft:
				_rightArm.Position = new Vector2(-2, -14);
				_leftArm.Position = new Vector2(-3, 8);
				break;
			case FacingDir.TopLeft:
				_rightArm.Position = new Vector2(0, 12);
				_leftArm.Position = new Vector2(-3, -10);
				break;
		}
	}

	public void SetIKTargets(NodePath right, NodePath left)
	{
		((SkeletonModification2DTwoBoneIK)_skeletonModifications.GetModification(0)).TargetNodePath = right;
		((SkeletonModification2DTwoBoneIK)_skeletonModifications.GetModification(1)).TargetNodePath = left;
	}

	private bool IsFacingRight()
	{
		return !_sprite.FlipH;
	}

	private void SetFacingDir(FacingDir newFacingDir)
	{
		_facingDir = newFacingDir;
		UpdateShoulderBones();
	}
}
