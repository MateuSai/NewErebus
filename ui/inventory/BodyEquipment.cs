using ErebusInventory;
using Godot;
using System;
using System.Collections.Generic;

namespace Erebus.UI.Inventory;

public partial class BodyEquipment : ItemInfo
{
    public Texture2D SpriteSheet;

    public List<Vector2I> InventoryGrid;

    static public BodyEquipment FromParameters(string id, Texture2D icon, int baseWidth, int baseHeight, Texture2D spriteSheet, List<Vector2I> inventoryGrid = null)
    {
        return new()
        {
            Id = id,
            Icon = icon,
            BaseWidth = baseWidth,
            BaseHeight = baseHeight,
            SpriteSheet = spriteSheet,
            InventoryGrid = inventoryGrid,
        };
    }
}
