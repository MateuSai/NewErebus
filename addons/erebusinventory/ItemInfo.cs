using Godot;
using System;

namespace ErebusInventory;

public partial class ItemInfo : GodotObject
{
    public readonly string Id;

    public Texture2D Icon;

    public int BaseWidth;
    public int BaseHeight;

    public ItemInfo(Texture2D icon, int baseWidth, int baseHeight)
    {
        Id = ((CSharpScript)GetScript()).ResourcePath.GetBaseName().GetFile();
        Icon = icon;
        BaseWidth = baseWidth;
        BaseHeight = baseHeight;
    }
}
