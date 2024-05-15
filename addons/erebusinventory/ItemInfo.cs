using Godot;
using System;
using System.Data;

namespace ErebusInventory;

public partial class ItemInfo : GodotObject
{
    public readonly string Id;

    public Texture2D Icon;

    private int _baseWidth;
    public int BaseWidth
    {
        get => Rotated ? _baseHeight : _baseWidth;
    }
    private int _baseHeight;
    public int BaseHeight
    {
        get => Rotated ? _baseWidth : _baseHeight;
    }

    public int Amount = 1;
    [Signal]
    public delegate void ItemRotatedEventHandler();
    private bool _rotated = false;
    public bool Rotated
    {
        get => _rotated;
        set
        {
            _rotated = value;
            EmitSignal(SignalName.ItemRotated);
        }
    }

    public ItemInfo(Texture2D icon, int baseWidth, int baseHeight)
    {
        Id = ((CSharpScript)GetScript()).ResourcePath.GetBaseName().GetFile();
        Icon = icon;
        _baseWidth = baseWidth;
        _baseHeight = baseHeight;
    }
}
