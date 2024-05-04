using Erebus.Autoloads;
using Erebus.Items;
using ErebusInventory;
using ErebusInventory.Grid;
using ErebusLogger;
using Godot;
using System;
using System.Collections.Generic;

namespace Erebus.UI.LootAndStashWindow;

public partial class LootGridInventory : GridInventory
{
    public readonly List<ItemOnFloor> ItemsOnFloor = new();

    public bool NotAddOrRemoveItemsOnFloor;

    private Globals _globals;

    public override void _Ready()
    {
        base._Ready();

        _globals = GetTree().Root.GetNode<Globals>("Globals");
    }

    public override void InsertItemByDragging(ItemInfo itemInfo, Vector2I atGridPos)
    {
        Log.Debug("InsertItemByDragging from LootGridInventory");

        base.InsertItemByDragging(itemInfo, atGridPos);

        if (!NotAddOrRemoveItemsOnFloor)
        {
            Log.Debug("Dropping item on floor...");
            ItemOnFloor itemOnFloor = new()
            {
                ItemInfoId = itemInfo.Id,
                Position = _globals.Player.Position
            };
            GetTree().CurrentScene.AddChild(itemOnFloor);

            ItemsOnFloor.Add(itemOnFloor);
        }
    }

    public override void RemoveItem(ItemInfo itemInfo, Vector2I atGridPos)
    {
        Log.Debug("RemoveItem from LootGridInventory");

        if (!NotAddOrRemoveItemsOnFloor)
        {
            ItemOnFloor itemOnFloor = ItemsOnFloor[Items.FindIndex(item => item == itemInfo)];
            ItemsOnFloor.Remove(itemOnFloor);
            itemOnFloor.QueueFree();
        }

        base.RemoveItem(itemInfo, atGridPos);
    }
}
