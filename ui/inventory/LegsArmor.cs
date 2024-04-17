using ErebusInventory;
using Godot;
using System;
using System.Collections.Generic;

namespace Erebus.UI.Inventory;

public partial class LegsArmor : BodyEquipment
{
    public LegsArmor(string id, Texture2D icon, int baseWidth, int baseHeight, Texture2D spriteSheet, List<Vector2I> inventoryGrid = null) : base(id, icon, baseWidth, baseHeight, spriteSheet, inventoryGrid)
    {
    }
}
