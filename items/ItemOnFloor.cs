using ErebusInventory;
using Godot;
using System;

namespace Erebus.Items;

[Tool, GlobalClass]
public partial class ItemOnFloor : Area2D
{
    private string _itemInfoId = null;
    [Export]
    public string ItemInfoId
    {
        get => _itemInfoId;
        set
        {
            _itemInfoId = value;
            // So it is not called before creating the sprite and collision shape.
            // It will be called later on the _ready function
            if (_sprite != null)
            {
                Initialize(_itemInfoId);
            }
        }
    }

    private Sprite2D _sprite;
    private CollisionShape2D _collisionShape;

    public override void _Ready()
    {
        base._Ready();

        CollisionLayer = 2; // Item
        CollisionMask = 0;

        _sprite = new();
        AddChild(_sprite);
        _collisionShape = new();
        AddChild(_collisionShape);

        Initialize(_itemInfoId);
    }

    /// <summary>
    /// Call this after _ready
    /// </summary>
    public void Initialize(string itemInfoId)
    {
        ItemInfo itemInfo = null;

        DirAccess itemsDir = DirAccess.Open("res://items");
        foreach (String dirName in itemsDir.GetDirectories())
        {
            itemInfo = SearchDirForItemId(DirAccess.Open(itemsDir.GetCurrentDir().PathJoin(dirName)), itemInfoId);
            if (itemInfo != null)
            {
                break;
            }
        }

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

    private ItemInfo SearchDirForItemId(DirAccess dir, string itemId)
    {
        ItemInfo itemInfo = null;

        foreach (string filename in dir.GetFiles())
        {
            if (filename.GetBaseName() == itemId)
            {
                CSharpScript script = GD.Load<CSharpScript>(dir.GetCurrentDir().PathJoin(itemId) + ".cs");
                itemInfo = (ItemInfo)script.New();
                break;
            }
        }

        return itemInfo;
    }
}
