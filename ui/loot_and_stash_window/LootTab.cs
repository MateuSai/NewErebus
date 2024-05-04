using Erebus.Autoloads;
using Erebus.Items;
using ErebusLogger;
using Godot;
using System.Collections.Generic;

namespace Erebus.UI.LootAndStashWindow;

public partial class LootTab : ScrollContainer
{
    private LootGridInventory _grid;

    public override void _Ready()
    {
        base._Ready();

        _grid = GetNode<LootGridInventory>("%GridInventory");

        GetVScrollBar().CustomMinimumSize = new(9, 0);
    }

    public void FillGrid()
    {
        _grid.NotAddOrRemoveItemsOnFloor = true;

        List<ItemOnFloor> nearItemsOnFloor = GetNode<Globals>("/root/Globals").Player.GetCloseItemsOnFloor();
        foreach (ItemOnFloor itemOnFloor in nearItemsOnFloor)
        {
            Log.Debug("Adding " + itemOnFloor.ItemInfo.Id);
            _grid.InsertItemAutomatically(itemOnFloor.ItemInfo);
            _grid.ItemsOnFloor.Add(itemOnFloor);
        }

        _grid.NotAddOrRemoveItemsOnFloor = false;
    }

    public void ClearGrid()
    {
        _grid.NotAddOrRemoveItemsOnFloor = true;

        GD.Print("clearing grid with " + _grid.Items.Count + " items...");
        for (int i = _grid.Items.Count - 1; i >= 0; i--)
        {
            GD.Print(i + ": " + _grid.Items[index: i] + "  at " + _grid.GridsWithItems[i]);
            _grid.GetCellAt(_grid.GridsWithItems[i]).Unequip();
            //_grid.RemoveItem(_grid.Items[index: i], _grid.GridsWithItems[i]);
        }
        _grid.ItemsOnFloor.Clear();

        _grid.NotAddOrRemoveItemsOnFloor = false;
    }
}
