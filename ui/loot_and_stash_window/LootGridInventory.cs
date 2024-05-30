using Erebus.Autoloads;
using Erebus.Items;
using ErebusInventory;
using ErebusInventory.Grid;
using ErebusLogger;
using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

    public override InsertResult InsertItemByDragging(ItemInfo itemInfo, Vector2I atGridPos, bool skipDivideWindow = false)
    {
        Log.Debug("InsertItemByDragging from LootGridInventory");

        InsertResult res = base.InsertItemByDragging(itemInfo, atGridPos, skipDivideWindow);
        if (res == InsertResult.Stacked || res == InsertResult.Cancelled || res == InsertResult.PartlyStacked || res == InsertResult.PartlyMoved)
        {
            return res;
        }

        if (!NotAddOrRemoveItemsOnFloor)
        {
            Log.Debug("Dropping item on floor...");
            ItemOnFloor itemOnFloor = new()
            {
                ItemInfo = itemInfo,
                Position = _globals.Player.Position
            };
            GetTree().CurrentScene.AddChild(itemOnFloor);

            ItemsOnFloor.Add(itemOnFloor);
        }

        return InsertResult.Moved;
    }

    public override void RemoveItem(ItemInfo itemInfo, Vector2I atGridPos, int width, int height)
    {
        Log.Debug("RemoveItem from LootGridInventory");

        if (!NotAddOrRemoveItemsOnFloor)
        {
            ItemOnFloor itemOnFloor = ItemsOnFloor[Items.FindIndex(item => item == itemInfo)];
            ItemsOnFloor.Remove(itemOnFloor);
            itemOnFloor.QueueFree();
        }

        base.RemoveItem(itemInfo, atGridPos, width, height);
    }
}
