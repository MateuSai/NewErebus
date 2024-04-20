using ErebusInventory;
using Godot;
using System;

namespace Erebus.Items;

[Tool, GlobalClass]
public partial class ItemOnFloor : Area2D
{
    private ItemInfo _itemInfo = null;
    [Export]
    public ItemInfo ItemInfo
    {
        get => _itemInfo;
        set
        {
            _itemInfo = value;
            // So it is not called before creating the sprite and collision shape.
            // It will be called later on the _ready function
            if (_sprite != null)
            {
                Initialize(_itemInfo);
            }
        }
    }

    private Sprite2D _sprite;
    private CollisionShape2D _collisionShape;

    public override void _Ready()
    {
        base._Ready();

        _sprite = new();
        AddChild(_sprite);
        _collisionShape = new();
        AddChild(_collisionShape);

        Initialize(_itemInfo);
    }

    /// <summary>
    /// Call this after _ready
    /// </summary>
    public void Initialize(ItemInfo itemInfo)
    {
        if (itemInfo != null)
        {
            _sprite.Texture = itemInfo.Icon;

            if (itemInfo.BaseWidth > itemInfo.BaseHeight)
            {
                CapsuleShape2D capsuleShape = new()
                {
                    Height = itemInfo.BaseWidth * 16,
                    Radius = (float)(itemInfo.BaseHeight / 2.0) * 16
                };
                _collisionShape.Shape = capsuleShape;
                RotationDegrees = 90;
            }
            else
            {
                CapsuleShape2D capsuleShape = new()
                {
                    Height = itemInfo.BaseHeight * 16,
                    Radius = (float)(itemInfo.BaseWidth / 2.0) * 16
                };
                _collisionShape.Shape = capsuleShape;
                RotationDegrees = 0;
            }
        }
        else
        {
            _sprite.Texture = null;
            _collisionShape.Shape = null;
        }
    }
}
