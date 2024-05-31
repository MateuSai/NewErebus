using ErebusLogger;
using Godot;
using Godot.Collections;
using System;
using System.Data;

namespace ErebusInventory;

public partial class ItemInfo : Resource
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

    public static ItemInfo FromJsonFile(string path)
    {
        return FromDic(LoadDicFromJsonFile(path));
    }

    public static ItemInfo FromDic(Dictionary dic)
    {
        return new ItemInfo(
            GD.Load<Texture2D>((string)dic["icon_path"]),
            (int)dic["width"],
            (int)dic["height"],
            dic.ContainsKey("capacity") ? (short)dic["capacity"] : (short)1
        );
    }

    private static Dictionary LoadDicFromJsonFile(string path)
    {
        FileAccess fileAccess = FileAccess.Open(path, FileAccess.ModeFlags.Read);
        if (fileAccess == null)
        {
            GD.PrintErr("Could not load json file");
            return null;
        }

        return (Dictionary)((Dictionary)Json.ParseString(fileAccess.GetAsText()))["data"];
    }

    /* public ItemInfo Duplicate()
    {
        return new ItemInfo(Icon, BaseWidth, BaseHeight, Capacity);
    } */

    public bool IsStackable()
    {
        return Capacity > 1;
    }

    /* public void DisconnectAllSignals()
    {
        foreach (Dictionary signalDic in GetSignalList())
        {
            Array<Dictionary> connectionList = GetSignalConnectionList((string)signalDic["name"]);
            foreach (Dictionary connectionDic in connectionList)
            {
                ((Signal)connectionDic["signal"]) -= (Callable)connectionDic["callable"];
            }
        }
    } */
}
