using Erebus.Autoloads;
using ErebusInventory;
using Godot;
using System;
using System.Collections.Generic;

namespace Erebus.UI.Inventory;

public partial class BackpackEquipmentItemSlot : BodyEquipmentEquipmentInventorySlot
{
    public override void _Ready()
    {
        GD.Print("backpack equipment slot ready");
        base._Ready();

        Equip(GD.Load<Backpack>("res://items/backpacks/InitialBackpack.tres"));
    }

    public override bool Equip(ItemInfo itemInfo)
    {
        if (itemInfo is not Backpack)
        {
            GD.Print("Item is not backpack");
            return false;
        }

        return base.Equip(itemInfo);
    }

    protected override void OnItemInfoChanged(ItemInfo itemInfo)
    {
        base.OnItemInfoChanged(itemInfo);

        GetNode<Globals>("/root/Globals").Player.SetBackpack((Backpack)itemInfo);

        if (itemInfo != null)
        {
            BodyEquipment bodyEquipment = (BodyEquipment)itemInfo;

            if (bodyEquipment.InventoryGrid != null)
            {
                AddInventoryGrid(bodyEquipment.InventoryGrid, GetOwner<Control>().GetNode("../InventoryWindow/BackpackInventory/VBoxContainer/MarginContainer/MarginContainer"));
            }
        }
    }
}
