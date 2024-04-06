using Godot;
using System;

namespace ErebusInventory.Grid;

public partial class GridItemSlot : Control, ItemSlot
{
    private readonly short _x;
    private readonly short _y;

    private ItemInfo _itemInfo;

    public TextureRect Icon;

    private InventorySystem _inventorySystem;

    public GridItemSlot(short x, short y)
    {
        _x = x;
        _y = y;

        SetAnchorsPreset(LayoutPreset.FullRect);
    }

    public override void _Ready()
    {
        base._Ready();

        _inventorySystem = GetNode<InventorySystem>("/root/" + nameof(InventorySystem));

        MouseEntered += () => _inventorySystem.AddSlotUnderMouse(this);
        MouseExited += () => _inventorySystem.RemoveSlotUnderMouse(this);
    }

    public void Equip(ItemInfo itemInfo)
    {
        _itemInfo = itemInfo;

        Icon.Texture = _itemInfo.Icon;
    }

    public TextureRect GetIconTextureRect()
    {
        throw new NotImplementedException();
    }

    public ItemInfo GetItemInfo()
    {
        throw new NotImplementedException();
    }

    public ItemInfo Grab()
    {
        throw new NotImplementedException();
    }
}
