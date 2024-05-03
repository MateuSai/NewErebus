using Erebus.Items;
using ErebusInventory;
using Godot;
using System;

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

    public override bool Equip(ItemInfo itemInfo)
    {
        if (itemInfo is not FrequencyJammer)
        {
            GD.Print("Item is not frequency jammer");
            return false;
        }

        return base.Equip(itemInfo);
    }

    protected override void OnItemInfoChanged(ItemInfo itemInfo)
    {
        base.OnItemInfoChanged(itemInfo);

        //GetNode<Globals>("/root/Globals").Player.SetBackpack((Backpack)itemInfo);
    }
}
