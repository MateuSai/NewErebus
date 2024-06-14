using Erebus.Autoloads;
using Erebus.Items;
using ErebusLogger;
using Godot;
using System.Collections.Generic;
using Log = ErebusLogger.Log;

namespace Erebus.UI.LootAndStashWindow;

public partial class LootTab : ScrollContainer
{
    public LootGridInventory Grid;

    public override void _Ready()
    {
        base._Ready();

        Grid = GetNode<LootGridInventory>("%GridInventory");

        GetVScrollBar().CustomMinimumSize = new(9, 0);
    }

    public void FillGrid()
    {
        Grid.NotAddOrRemoveItemsOnFloor = true;

        List<ItemOnFloor> nearItemsOnFloor = GetNode<Globals>("/root/Globals").Player.GetCloseItemsOnFloor();
        Log.Debug("Number of near items on floor: " + nearItemsOnFloor.Count);
        foreach (ItemOnFloor itemOnFloor in nearItemsOnFloor)
        {
            Log.Debug("Adding " + itemOnFloor.ItemInfo.Id);
            Grid.InsertItemAutomatically(itemOnFloor.ItemInfo);
            Grid.ItemsOnFloor.Add(itemOnFloor);
        }

        Grid.NotAddOrRemoveItemsOnFloor = false;
    }

    public void ClearGrid()
    {
        Grid.NotAddOrRemoveItemsOnFloor = true;

        GD.Print("clearing grid with " + Grid.Items.Count + " items...");
        for (int i = Grid.Items.Count - 1; i >= 0; i--)
        {
            GD.Print(i + ": " + Grid.Items[index: i] + "  at " + Grid.GridsWithItems[i]);
            Grid.GetCellAt(Grid.GridsWithItems[i]).Unequip();
            //_grid.RemoveItem(_grid.Items[index: i], _grid.GridsWithItems[i]);
        }
        Grid.ItemsOnFloor.Clear();

        Grid.NotAddOrRemoveItemsOnFloor = false;
    }
}
