using ErebusInventory;
using Godot;
using System;

namespace Erebus.UI.Inventory;

public partial class Backpack : ItemInfo
{
    public Texture2D SpriteSheet;

    public Backpack(string id, Texture2D icon, int baseWidth, int baseHeight, Texture2D spriteSheet) : base(id, icon, baseWidth, baseHeight)
    {
        SpriteSheet = spriteSheet;
    }
}
