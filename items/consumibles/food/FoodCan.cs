using ErebusInventory;
using Godot;
using System;

namespace Erebus.Items.Consumibles.Food;

public partial class FoodCan : ItemInfo
{
    public FoodCan() : base(GD.Load<Texture2D>("res://art/ui/inventory_icons/Food_can.png"), 1, 2)
    {
    }
}
