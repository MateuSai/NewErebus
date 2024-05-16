using ErebusInventory;
using Godot;
using System;

namespace Erebus.Items.Ammo.Boxes;

public partial class AmmoBox9x18 : AmmoBox
{
    public AmmoBox9x18() : base(GD.Load<Texture2D>("res://art/ui/inventory_icons/9x18mm_ammo_box.png"), 1, 1, 50)
    {
        Amount = 35;
    }
}
