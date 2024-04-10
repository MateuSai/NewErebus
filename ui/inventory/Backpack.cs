using ErebusInventory;
using Godot;
using System;

namespace Erebus.UI.Inventory;

public partial class Backpack : ItemInfo
{
    public Backpack(string id, Texture2D icon, int baseWidth, int baseHeight) : base(id, icon, baseWidth, baseHeight)
    {
    }
}
