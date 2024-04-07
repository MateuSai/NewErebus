using Godot;
using System;

namespace ErebusInventory.Grid;

public partial class GridItemSlotReference : Control, IItemSlot
{
    private GridItemSlot _gridItemSlot;

    public GridItemSlotReference(GridItemSlot gridItemSlot)
    {
        _gridItemSlot = gridItemSlot;
    }

    public void Equip(ItemInfo itemInfo)
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
        return _gridItemSlot.Grab();
    }

    public void Unequip()
    {
        _gridItemSlot.Unequip();
    }
}
