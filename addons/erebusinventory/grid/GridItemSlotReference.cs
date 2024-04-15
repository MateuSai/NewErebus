using Godot;
using System;

namespace ErebusInventory.Grid;

public partial class GridItemSlotReference : Control, IItemSlot
{
    private GridItemSlot _gridItemSlot;

    private InventorySystem _inventorySystem;

    public GridItemSlotReference(GridItemSlot gridItemSlot)
    {
        _gridItemSlot = gridItemSlot;

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
        throw new NotImplementedException();
    }

    public TextureRect GetIconTextureRect()
    {
        return _gridItemSlot.GetIconTextureRect();
    }

    public ItemInfo GetItemInfo()
    {
        return _gridItemSlot.GetItemInfo();
    }

    public ItemInfo Grab()
    {
        //GD.Print("hoho");
        return _gridItemSlot.Grab();
    }

    public void Unequip()
    {
        _gridItemSlot.Unequip();
    }
}
