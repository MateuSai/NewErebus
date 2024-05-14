using Erebus.Autoloads;
using ErebusInventory;
using ErebusLogger;
using Godot;
using System;
using System.Reflection.Metadata.Ecma335;

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

        Equip(new LegsArmor(GD.Load<Texture2D>("res://art/ui/inventory_icons/Jeans_and_boots.png"), 2, 2, GD.Load<Texture2D>("res://art/player_equipment/pants/Jeans_and_boots.png")));
    }

    public new bool CanEquip(ItemInfo itemInfo)
    {
        if (itemInfo is not LegsArmor)
        {
            Log.Debug("Item is not legs armor");
            return false;
        }

        return base.CanEquip(itemInfo);
    }

    public override void Equip(ItemInfo itemInfo)
    {
        if (!CanEquip(itemInfo))
        {
            Log.Fatal("Tried to equip item on slot when CanEquip returns false", GetTree());
        }

        base.Equip(itemInfo);
    }
}
