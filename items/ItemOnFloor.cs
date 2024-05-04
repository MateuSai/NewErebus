using ErebusInventory;
using ErebusLogger;
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

    public ItemInfo ItemInfo = null;

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
        DirAccess itemsDir = DirAccess.Open("res://items");
        ItemInfo = SearchSubDirsForItem(itemsDir, itemInfoId);


        if (ItemInfo != null)
        {
            _sprite.Texture = ItemInfo.Icon;

            if (ItemInfo.BaseWidth > ItemInfo.BaseHeight)
            {
                CapsuleShape2D capsuleShape = new()
                {
                    Height = ItemInfo.BaseWidth * 16,
                    Radius = (float)(ItemInfo.BaseHeight / 2.0) * 16
                };
                _collisionShape.Shape = capsuleShape;
                _collisionShape.RotationDegrees = 90;
            }
            else
            {
                CapsuleShape2D capsuleShape = new()
                {
                    Height = ItemInfo.BaseHeight * 16,
                    Radius = (float)(ItemInfo.BaseWidth / 2.0) * 16
                };
                _collisionShape.Shape = capsuleShape;
                _collisionShape.RotationDegrees = 0;
            }
        }
        else
        {
            _sprite.Texture = null;
            _collisionShape.Shape = null;

            if (!Engine.IsEditorHint())
            {
                GD.PushError("Invalid item: " + ItemInfoId);
            }
        }
    }

    private ItemInfo SearchSubDirsForItem(DirAccess dir, string itemInfoId)
    {
        Log.Debug("Checking " + dir.GetCurrentDir());

        ItemInfo itemInfo = SearchDirForItem(DirAccess.Open(dir.GetCurrentDir()), itemInfoId);

        if (itemInfo != null)
        {
            return itemInfo;
        }

        foreach (string dirName in dir.GetDirectories())
        {
            //ItemInfo = SearchDirForItem(DirAccess.Open(dir.GetCurrentDir().PathJoin(dirName)), itemInfoId);
            //if (ItemInfo != null)
            //{
            //  break;
            //}
            //else
            //{
            itemInfo = SearchSubDirsForItem(DirAccess.Open(dir.GetCurrentDir().PathJoin(dirName)), itemInfoId);
            if (itemInfo != null)
            {
                break;
            }
            //}
        }

        return itemInfo;
    }

    private ItemInfo SearchDirForItem(DirAccess dir, string itemId)
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
