using Erebus.UI.Inventory;
using Godot;
using Godot.Collections;
using Godot.NativeInterop;
using System;

namespace Erebus.Items.Backpacks;

public partial class InitialBackpack : Backpack
{
    public InitialBackpack() : base(GD.Load<Texture2D>(path: "res://art/ui/inventory_icons/Backpack_base.png"), 2, 2, GD.Load<Texture2D>("res://art/player_equipment/bags/Backpack_base.png"), new Godot.Collections.Array<Vector2I> { new(2, 3), new(5, 7), new(2, 3) })
    {
    }
}
