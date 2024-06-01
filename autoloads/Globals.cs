using Erebus.Characters.Player;
using Erebus.UI;
using ErebusInventory;
using ErebusLogger;
using Godot;
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

        // Mod mod = ModLoader.LoadMod(ProjectSettings.GlobalizePath("res://mods/TestMod"));
        //Log.Debug(mod.ToString());
        //Log.Debug(ModLoader.LoadedMods["test_mod"].ToString());
    }

    public void AddModItem(string id, string path)
    {
        Log.Info("Adding mod item " + id + " at " + path);
        _modItems.Add(id, path);
        foreach (KeyValuePair<string, string> kvp in _modItems)
        {
            //textBox3.Text += ("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
            Log.Debug("Key = " + kvp.Key + ", Value = " + kvp.Value);
        }
    }

    public ItemInfo GetModItem(string id)
    {
        Log.Debug("Trying to get mod item: " + id);
        return _modItems.ContainsKey(id) ? ItemInfo.FromJsonFile(_modItems[id]) : null;
    }
}
