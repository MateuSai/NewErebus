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

        VisibilityChanged += () =>
        {
            if (Visible)
            {
                FillGrid();
            }
        };
        Hidden += ClearGrid;
    }

    private void FillGrid()
    {
        List<ItemOnFloor> nearItemsOnFloor = GetNode<Globals>("/root/Globals").Player.GetCloseItemsOnFloor();
        foreach (ItemOnFloor itemOnFloor in nearItemsOnFloor)
        {
            _grid.InsertItemAutomatically(itemOnFloor.ItemInfo);
        }
    }

    private void ClearGrid()
    {
        GD.Print("sdfd");
        for (int i = _grid.Items.Count - 1; i >= 0; i--)
        {
            _grid.RemoveItem(_grid.Items[i], _grid.GridsWithItems[i]);
        }
    }
}
