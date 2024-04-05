using Godot;
using System;

namespace ErebusInventory;

public partial class ItemInfo
{
    public readonly string Id;

    public Texture2D Icon;

    public readonly int BaseWidth;
    public readonly int BaseHeight;

    public ItemInfo(string id, Texture2D icon, int baseWidth, int baseHeight)
    {
        this.Id = id;
        this.Icon = icon;
        this.BaseWidth = baseWidth;
        this.BaseHeight = baseHeight;
    }
}
