using Erebus.Autoloads;
using ErebusInventory;
using Godot;
using System;

namespace Erebus.UI.Inventory;

public partial class HelmetEquipmentItemSlot : EquipmentItemSlot
{
    public override void _Ready()
    {
        //GD.Print("backpack equipment slot ready");
        base._Ready();

        ItemInfoChanged += (ItemInfo itemInfo) =>
        {
            //GetNode<Globals>("/root/Globals").Player.SetBackpack((Backpack)itemInfo);
        };

        //Equip(new Backpack("id", GD.Load<Texture2D>("res://art/ui/inventory_icons/Backpack_base.png"), 2, 2, GD.Load<Texture2D>("res://art/player_equipment/bags/Backpack_base.png")));
    }

    public override bool Equip(ItemInfo itemInfo)
    {
        if (itemInfo is not Helmet)
        {
            GD.Print("Item is not helmet");
            return false;
        }

        return base.Equip(itemInfo);
    }
}
