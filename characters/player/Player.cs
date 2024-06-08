namespace Erebus.Characters.Player;

using Erebus.Autoloads;
using Erebus.Items;
using Erebus.UI.Inventory;
using Erebus.Weapons;
using Godot;

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
	private enum HorizontalFacingDir
	{
		Left,
		Right,
	}
	private HorizontalFacingDir _horizontalFacingDir = HorizontalFacingDir.Left;
	private enum VerticalFacingDir
	{
		Top,
		Bottom,
	}
	private VerticalFacingDir _verticalFacingDir = VerticalFacingDir.Bottom;

	private PlayerStateMachine _stateMachine;

	private Sprite2D _backpackSprite;
	private Sprite2D _legsArmorSprite;
	private Sprite2D _sprite;
	private AnimationPlayer _animationPlayer;
	private PlayerWeapons _weapons;
	private ItemsOnFloorDetector _itemsOnFloorDetector;

	public override void _Ready()
	{
		base._Ready();

		_backpackSprite = GetNode<Sprite2D>("Backpack");
		_legsArmorSprite = GetNode<Sprite2D>("LegsArmorSprite");
		_sprite = GetNode<Sprite2D>("Sprite2D");
		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		_weapons = GetNode<PlayerWeapons>("Weapons");
		_itemsOnFloorDetector = GetNode<ItemsOnFloorDetector>("ItemsOnFloorDetector");

		_stateMachine = GetNode<PlayerStateMachine>("StateMachine");

		_weapons.Start();

		GetNode<Globals>("/root/Globals").Player = this;
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
				break;
			case FacingDir.TopRight:
				break;
			case FacingDir.BottomLeft:
				break;
			case FacingDir.TopLeft:
				break;
		}
	}

	private bool IsFacingRight()
	{
		return !_sprite.FlipH;
	}

	private void SetFacingDir(FacingDir newFacingDir)
	{
		if ((newFacingDir == FacingDir.TopLeft || newFacingDir == FacingDir.BottomLeft) && (_facingDir == FacingDir.TopRight || _facingDir == FacingDir.BottomRight))
		{
			SetHorizontalFacingDir(HorizontalFacingDir.Left);
		}
		else if ((newFacingDir == FacingDir.TopRight || newFacingDir == FacingDir.BottomRight) && (_facingDir == FacingDir.TopLeft || _facingDir == FacingDir.BottomLeft))
		{
			SetHorizontalFacingDir(HorizontalFacingDir.Right);
		}

		if ((newFacingDir == FacingDir.TopLeft || newFacingDir == FacingDir.TopRight) && (_facingDir == FacingDir.BottomLeft || _facingDir == FacingDir.BottomRight))
		{
			SetVerticalFacingDir(VerticalFacingDir.Top);
		}
		else if ((newFacingDir == FacingDir.BottomLeft || newFacingDir == FacingDir.BottomRight) && (_facingDir == FacingDir.TopLeft || _facingDir == FacingDir.TopRight))
		{
			SetVerticalFacingDir(VerticalFacingDir.Bottom);
		}

		_facingDir = newFacingDir;
		UpdateShoulderBones();
	}

	private void SetHorizontalFacingDir(HorizontalFacingDir newHorizontalFacingDir)
	{
		_horizontalFacingDir = newHorizontalFacingDir;

		switch (_horizontalFacingDir)
		{
			case HorizontalFacingDir.Right:
				_sprite.FlipH = false;
				_backpackSprite.FlipH = false;
				_legsArmorSprite.FlipH = false;
				break;
			case HorizontalFacingDir.Left:
				_sprite.FlipH = true;
				_backpackSprite.FlipH = true;
				_legsArmorSprite.FlipH = true;
				break;
			default:
				System.Diagnostics.Debug.Assert(false, "Invalid horizontal facing direction value");
				break;
		}
	}

	private void SetVerticalFacingDir(VerticalFacingDir newVerticalFacingDir)
	{
		_verticalFacingDir = newVerticalFacingDir;

		switch (_verticalFacingDir)
		{
			case VerticalFacingDir.Bottom:
				MoveChild(_backpackSprite, _sprite.GetIndex() - 1);
				break;
			case VerticalFacingDir.Top:
				MoveChild(_backpackSprite, _sprite.GetIndex() + 1);
				break;
			default:
				System.Diagnostics.Debug.Assert(false, "Invalid vertical facing direction value");
				break;
		}
	}

	public void SetBackpack(Backpack backpack)
	{
		if (backpack == null)
		{
			_backpackSprite.Texture = null;
		}
		else
		{
			_backpackSprite.Texture = backpack.SpriteSheet;
		}
	}

	public void SetLegsArmor(LegsArmor legsArmor)
	{
		if (legsArmor == null)
		{
			_legsArmorSprite.Texture = null;
		}
		else
		{
			_legsArmorSprite.Texture = legsArmor.SpriteSheet;
		}
	}

	public System.Collections.Generic.List<ItemOnFloor> GetCloseItemsOnFloor()
	{
		return _itemsOnFloorDetector.GetItemsOnFloor();
	}
}
