using ErebusInventory;
using Godot;
using System;

namespace Erebus.Items.Others;

public partial class GasCan : ItemInfo
{
    public GasCan() : base(GD.Load<Texture2D>("res://art/ui/inventory_icons/Gas_can.png"), 3, 2)
    {
    }
}
