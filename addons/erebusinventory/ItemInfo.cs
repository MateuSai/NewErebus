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

    public short Capacity;
    [Signal]
    public delegate void AmountChangedEventHandler(int newAmount);
    private int _amount = 1;
    public int Amount
    {
        get => _amount;
        set
        {
            _amount = value;
            EmitSignal(SignalName.AmountChanged, _amount);
        }
    }
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

    public ItemInfo(Texture2D icon, int baseWidth, int baseHeight, short capacity = 1)
    {
        Id = ((CSharpScript)GetScript()).ResourcePath.GetBaseName().GetFile();
        Icon = icon;
        _baseWidth = baseWidth;
        _baseHeight = baseHeight;
        Capacity = capacity;
    }

    public bool IsStackable()
    {
        return Capacity > 1;
    }
}
