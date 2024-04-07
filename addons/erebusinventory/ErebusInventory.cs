#if TOOLS
using Godot;
using System;

namespace ErebusInventory;

[Tool]
public partial class ErebusInventory : EditorPlugin
{
    public override void _EnterTree()
    {
        // Initialization of the plugin goes here.
        AddCustomType(nameof(IItemSlot), nameof(Container), GD.Load<Script>("res://addons/erebusinventory/ItemSlot.cs"), GD.Load<Texture2D>("res://icon.svg"));
        AddAutoloadSingleton(nameof(InventorySystem), "res://addons/erebusinventory/InventorySystem.cs");
    }

    public override void _ExitTree()
    {
        // Clean-up of the plugin goes here.
        RemoveCustomType(nameof(IItemSlot));
        RemoveAutoloadSingleton(nameof(InventorySystem));
    }
}
#endif
