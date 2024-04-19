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
        }
    }

    private Sprite2D _sprite;
    private CollisionShape2D _collisionShape;

    public override void _Ready()
    {
        base._Ready();

        _sprite = GetNode<Sprite2D>("Sprite2D");
        _collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");

        if (_itemInfo != null)
        {
            Initialize(_itemInfo);
        }
    }

    /// <summary>
    /// Call this after _ready
    /// </summary>
    public void Initialize(ItemInfo itemInfo)
    {
        _sprite.Texture = itemInfo.Icon;

        RectangleShape2D rectangleShape = new()
        {
            Size = itemInfo.Icon.GetSize()
        };
        _collisionShape.Shape = rectangleShape;
    }
}
