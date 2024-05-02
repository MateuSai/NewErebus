using Erebus.Autoloads;
using Erebus.Items;
using ErebusInventory.Grid;
using Godot;
using System;
using System.Collections.Generic;
using System.Security.Principal;

namespace Erebus.UI.LootAndStashWindow;

public partial class LootTab : ScrollContainer
{
    private GridInventory _grid;

    public override void _Ready()
    {
        base._Ready();

        _grid = GetNode<GridInventory>("%GridInventory");

        GetVScrollBar().CustomMinimumSize = new(9, 0);

        Draw += () =>
        {
            FillGrid();
        };
        Hidden += ClearGrid;
    }

    private void FillGrid()
    {
        List<ItemOnFloor> nearItemsOnFloor = GetNode<Globals>("/root/Globals").Player.GetCloseItemsOnFloor();
        foreach (ItemOnFloor itemOnFloor in nearItemsOnFloor)
        {
            GD.Print("Adding " + itemOnFloor.ItemInfo.Id);
            _grid.InsertItemAutomatically(itemOnFloor.ItemInfo);
        }
    }

    private void ClearGrid()
    {
        GD.Print("clearing grid with " + _grid.Items.Count + " items...");
        for (int i = _grid.Items.Count - 1; i >= 0; i--)
        {
            GD.Print(i + ": " + _grid.Items[index: i] + "  at " + _grid.GridsWithItems[i]);
            _grid.GetCellAt(_grid.GridsWithItems[i]).Unequip();
            //_grid.RemoveItem(_grid.Items[index: i], _grid.GridsWithItems[i]);
        }
    }
}
