using Erebus.UI.Inventory;
using Godot;
using Godot.Collections;
using System;

public partial class JeansAndBoots : LegsArmor
{
    public JeansAndBoots() : base(GD.Load<Texture2D>("res://art/ui/inventory_icons/Jeans_and_boots.png"), 2, 2, GD.Load<Texture2D>("res://art/player_equipment/pants/Jeans_and_boots.png"), new Array<Vector2I> { new(2, 2), new(2, 2) })
    {
    }
}
