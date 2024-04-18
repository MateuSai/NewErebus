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

        Equip(new Backpack("id", GD.Load<Texture2D>("res://art/ui/inventory_icons/Backpack_base.png"), 2, 2, GD.Load<Texture2D>("res://art/player_equipment/bags/Backpack_base.png"), new List<Vector2I> { new(2, 3), new(5, 7), new(2, 3) }));
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

        BodyEquipment bodyEquipment = (BodyEquipment)itemInfo;

        if (bodyEquipment.InventoryGrid != null)
        {
            AddInventoryGrid(bodyEquipment.InventoryGrid, GetOwner<Control>().GetNode("../InventoryWindow/BackpackInventory/VBoxContainer"));
        }
    }
}
