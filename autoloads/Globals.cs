using Erebus.Characters.Player;
using Erebus.UI;
using ErebusInventory;
using ErebusLogger;
using Godot;
using Godot.Modding;
using System;
using System.Collections.Generic;
using System.Linq;
using Log = ErebusLogger.Log;

namespace Erebus.Autoloads;

public partial class Globals : Node
{
    public Player Player;
    public UI.UI UI;

    private Dictionary<string, string> _modItems = new();

    public override void _Ready()
    {
        base._Ready();

        Mod mod = ModLoader.LoadMod(ProjectSettings.GlobalizePath("res://mods/TestMod"));
        Log.Debug(mod.ToString());
        //Log.Debug(ModLoader.LoadedMods["test_mod"].ToString());
    }

    public void AddModItem(string id, string path)
    {
        _modItems.Add(id, path);
    }

    public ItemInfo GetModItem(string id)
    {
        return _modItems.ContainsKey(id) ? ItemInfo.FromJsonFile(_modItems[id]) : null;
    }
}
