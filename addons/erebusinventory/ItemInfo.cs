using Godot;
using System;

namespace ErebusInventory;

[Tool]
public partial class ItemInfo : Resource
{
    [Export]
    public string Id;

    [Export]
    public Texture2D Icon;

    [Export]
    public int BaseWidth;
    [Export]
    public int BaseHeight;

    public ItemInfo()
    {
    }

    static public ItemInfo FromParameters(string id, Texture2D icon, int baseWidth, int baseHeight)
    {
        return new()
        {
            Id = id,
            Icon = icon,
            BaseWidth = baseWidth,
            BaseHeight = baseHeight,
        };
    }
}
