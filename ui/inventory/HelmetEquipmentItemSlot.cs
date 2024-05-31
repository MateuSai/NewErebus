using Erebus.Autoloads;
using ErebusInventory;
using ErebusLogger;
using Godot;
using System;
using System.Threading.Tasks;
using Log = ErebusLogger.Log;

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

    public new bool CanEquip(ItemInfo itemInfo)
    {
        if (itemInfo is not Helmet)
        {
            Log.Debug("Item is not helmet");
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
