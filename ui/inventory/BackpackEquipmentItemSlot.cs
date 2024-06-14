using Erebus.Autoloads;
using Erebus.Items.Backpacks;
using ErebusInventory;
using ErebusLogger;
using Godot;
using System;
using System.Collections.Generic;
using Log = ErebusLogger.Log;

namespace Erebus.UI.Inventory;

public partial class BackpackEquipmentItemSlot : BodyEquipmentEquipmentInventorySlot
{
    public override void _Ready()
    {
        GD.Print("backpack equipment slot ready");
        base._Ready();

        Equip(new InitialBackpack());
    }

    public override bool CanEquip(ItemInfo itemInfo)
    {
        if (itemInfo is not Backpack)
        {
            Log.Debug("Item is not backpack");
            return false;
        }

        return base.CanEquip(itemInfo);
    }

    protected override void OnItemInfoChanged(ItemInfo itemInfo)
    {
        base.OnItemInfoChanged(itemInfo);

        GetNode<Globals>("/root/Globals").Player.SetBackpack((Backpack)itemInfo);
    }
}
