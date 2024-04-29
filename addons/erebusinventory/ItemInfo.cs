using Godot;
using System;

namespace ErebusInventory;

public partial class ItemInfo : GodotObject
{
    public string Id;

    public Texture2D Icon;

    public int BaseWidth;
    public int BaseHeight;

    public ItemInfo(string id, Texture2D icon, int baseWidth, int baseHeight)
    {
        Id = id;
        Icon = icon;
        BaseWidth = baseWidth;
        BaseHeight = baseHeight;
    }
}
