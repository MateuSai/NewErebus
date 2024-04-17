using Godot;
using System;

namespace ErebusInventory.Grid;

public partial class GridItemSlot : Control, IItemSlot
{
    private readonly short _x;
    private readonly short _y;

    private ItemInfo _itemInfo;

    public TextureRect Icon;

    private InventorySystem _inventorySystem;

    private GridInventory _gridInventory;

    public GridItemSlot(short x, short y, GridInventory gridInventory)
    {
        _x = x;
        _y = y;
        _gridInventory = gridInventory;
        MouseFilter = MouseFilterEnum.Pass;

        Icon = new()
        {
            StretchMode = TextureRect.StretchModeEnum.Keep,
            ZIndex = 1,
            MouseFilter = MouseFilterEnum.Ignore,
        };
        AddChild(Icon);

        SetAnchorsPreset(LayoutPreset.FullRect);
    }

    public override void _Ready()
    {
        base._Ready();

        _inventorySystem = GetNode<InventorySystem>("/root/" + nameof(InventorySystem));

        MouseEntered += () => _inventorySystem.AddSlotUnderMouse(this);
        MouseExited += () => _inventorySystem.RemoveSlotUnderMouse(this);
    }

    public bool Equip(ItemInfo itemInfo)
    {
        //GD.Print(_x + " " + _y);

        bool couldInsert = _gridInventory.InsertItem(itemInfo, new Vector2I(_x, _y));

        if (!couldInsert)
        {
            return false;
        }

        _itemInfo = itemInfo;

        Icon.Texture = _itemInfo.Icon;

        return true;
    }

    public TextureRect GetIconTextureRect()
    {
        return (TextureRect)Icon.Duplicate();
    }

    public ItemInfo GetItemInfo()
    {
        return _itemInfo;
    }

    public ItemInfo Grab()
    {
        GD.Print(_x + " " + _y);
        return _itemInfo;
    }

    public void Unequip()
    {
        //GD.Print("hey");
        _gridInventory.RemoveItem(_itemInfo, new Vector2I(_x, _y));

        Icon.Texture = null;
        _itemInfo = null;
    }
}
