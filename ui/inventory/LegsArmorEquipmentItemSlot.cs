using Erebus.Autoloads;
using ErebusInventory;
using ErebusLogger;
using Godot;
using System;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Log = ErebusLogger.Log;

namespace Erebus.UI.Inventory;

public partial class LegsArmorEquipmentItemSlot : BodyEquipmentEquipmentInventorySlot
{
    public override void _Ready()
    {
        //GD.Print("backpack equipment slot ready");

        type = InventoryWindow.InventoryWindow.Type.Legs;

        base._Ready();

        ItemInfoChanged += (ItemInfo itemInfo) =>
        {
            GetNode<Globals>("/root/Globals").Player.SetLegsArmor((LegsArmor)itemInfo);
        };

        Equip(new JeansAndBoots());
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
