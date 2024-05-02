using ErebusInventory;
using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

namespace Erebus.UI.Inventory;

public partial class Backpack : BodyEquipment
{
    public Backpack(Texture2D icon, int baseWidth, int baseHeight, Texture2D spriteSheet, Array<Vector2I> inventoryGrid = null) : base(icon, baseWidth, baseHeight, spriteSheet, inventoryGrid)
    {
    }
}
