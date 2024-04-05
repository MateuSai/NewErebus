using Godot;
using System;

namespace ErebusInventory;

[Tool]
public partial class ItemSlot : Container
{
    private ItemInfo _itemInfo;

    public void Equip(ItemInfo itemInfo)
    {
        _itemInfo = itemInfo;
    }
}
