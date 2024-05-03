using ErebusInventory;
using Godot;
using System;

namespace Erebus.Items.Consumibles.Food;

public partial class SmallFoodCan : ItemInfo
{
    public SmallFoodCan() : base(GD.Load<Texture2D>("res://art/ui/inventory_icons/Food_can_small.png"), 1, 1)
    {
    }
}
