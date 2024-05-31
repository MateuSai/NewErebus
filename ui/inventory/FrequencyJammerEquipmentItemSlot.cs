using Erebus.Items;
using ErebusInventory;
using ErebusLogger;
using Godot;
using System;
using System.Threading.Tasks;
using Log = ErebusLogger.Log;

namespace Erebus.UI;

[GlobalClass]
public partial class FrequencyJammerEquipmentItemSlot : EquipmentItemSlot
{
    public override void _Ready()
    {
        GD.Print("frequency jammer equipment slot ready");

        base._Ready();

        ItemInfoChanged += OnItemInfoChanged;

        Equip(new FrequencyJammer());
    }

    public new bool CanEquip(ItemInfo itemInfo)
    {
        if (itemInfo is not FrequencyJammer)
        {
            Log.Debug("Item is not frequency jammer");
            return false;
        }

        return true;
    }

    public override void Equip(ItemInfo itemInfo)
    {
        if (!CanEquip(itemInfo))
        {
            Log.Fatal("Tried to equip item on slot when CanEquip returns false", GetTree());
        }

        base.Equip(itemInfo);
    }

    protected override void OnItemInfoChanged(ItemInfo itemInfo)
    {
        base.OnItemInfoChanged(itemInfo);

        //GetNode<Globals>("/root/Globals").Player.SetBackpack((Backpack)itemInfo);
    }
}
