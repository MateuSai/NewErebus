using ErebusInventory;
using Godot;
using System;
using System.Collections.Generic;

namespace Erebus.UI.Inventory;

public partial class BodyEquipment : ItemInfo
{
    public Texture2D SpriteSheet;

    public Godot.Collections.Array<Vector2I> InventoryGrid;

    public BodyEquipment(string id, Texture2D icon, int baseWidth, int baseHeight, Texture2D spriteSheet, Godot.Collections.Array<Vector2I> inventoryGrid = null) : base(id, icon, baseWidth, baseHeight)
    {
        SpriteSheet = spriteSheet;
        InventoryGrid = inventoryGrid;
    }
}
