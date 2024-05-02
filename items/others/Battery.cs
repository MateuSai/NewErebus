using ErebusInventory;
using Godot;
using System;

namespace Erebus.Items.Others;

public partial class Battery : ItemInfo
{
    public Battery() : base("battery", GD.Load<Texture2D>("res://art/ui/inventory_icons/Battery_01.png"), 1, 1)
    {
    }
}

