using Erebus.Autoloads;
using ErebusInventory;
using Godot;
using System;

namespace Erebus.UI.Inventory;

public partial class LegsArmorEquipmentItemSlot : EquipmentItemSlot
{
    public override void _Ready()
    {
        //GD.Print("backpack equipment slot ready");
        base._Ready();

        ItemInfoChanged += (ItemInfo itemInfo) =>
        {
            GetNode<Globals>("/root/Globals").Player.SetLegsArmor((LegsArmor)itemInfo);
        };

        Equip(new LegsArmor("id", GD.Load<Texture2D>("res://art/ui/inventory_icons/Jeans_and_boots.png"), 2, 2, GD.Load<Texture2D>("res://art/player_equipment/pants/Jeans_and_boots.png")));
    }

    public override bool Equip(ItemInfo itemInfo)
    {
        if (itemInfo is not LegsArmor)
        {
            GD.Print("Item is not legs armor");
            return false;
        }

        return base.Equip(itemInfo);
    }
}
