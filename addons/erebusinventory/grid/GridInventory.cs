using Godot;
using System;
using System.Collections.Generic;

namespace ErebusInventory.Grid;

[GlobalClass, Tool]
public partial class GridInventory : GridContainer
{
    [Export]
    public int Rows;
    [Export]
    private int _cellSize = 16;

    private IItemSlot[] _grid;

    private InventorySystem _inventorySystem;

    public GridInventory()
    {
        AddThemeConstantOverride("h_separation", 0);
        AddThemeConstantOverride("v_separation", 0);
    }

    public override void _Ready()
    {
        base._Ready();

        _inventorySystem = GetNode<InventorySystem>("/root/" + nameof(InventorySystem));

        CreateGrid();
    }

    public void CreateGrid()
    {
        for (int i = GetChildCount() - 1; i >= 0; i--)
        {
            GetChild(i).QueueFree();
        }

        _grid = new IItemSlot[Columns * Rows];

        for (int j = 0; j < Rows * Columns; j++)
        {
            GridCell gridCell = new();

            GridItemSlot gridItemSlot = new((short)(j % Rows), (short)(j / Columns), this);
            gridCell.AddChild(gridItemSlot);
            _grid[j] = gridItemSlot;

            AddChild(gridCell);
        }
    }

    public bool InsertItem(ItemInfo itemInfo, Vector2I atGridPos)
    {
        //GD.Print("Inserting " + itemInfo + " at " + atGridPos);

        if (!IsGridPositionValid(atGridPos))
        {
            return false;
        }

        List<Vector2I> gridPositions = new();

        for (int x = 1; x < itemInfo.BaseWidth; x++)
        {
            for (int y = 1; y < itemInfo.BaseHeight; y++)
            {
                gridPositions.Add(new Vector2I(atGridPos.X + x, atGridPos.Y + y));
            }
        }

        //GD.Print("Positions to check " + gridPositions.Count);

        foreach (Vector2I pos in gridPositions)
        {
            //GD.Print("Checking " + pos);
            if (!IsGridPositionValid(pos))
            {
                return false;
            }
        }

        foreach (Vector2I pos in gridPositions)
        {
            GridItemSlotReference gridItemSlotReference = new((GridItemSlot)GetSlotAt(atGridPos));
            ((Control)GetSlotAt(pos)).GetParent().AddChild(gridItemSlotReference);
            ((Control)GetSlotAt(pos)).QueueFree();
            _grid[(int)(pos.Y * Columns + pos.X)] = gridItemSlotReference;
        }

        return true;
    }

    public void RemoveItem(ItemInfo itemInfo, Vector2I atGridPos)
    {
        List<Vector2I> gridPositions = new();

        for (int x = 1; x < itemInfo.BaseWidth; x++)
        {
            for (int y = 1; y < itemInfo.BaseHeight; y++)
            {
                gridPositions.Add(new Vector2I(atGridPos.X + x, atGridPos.Y + y));
            }
        }

        foreach (Vector2I pos in gridPositions)
        {
            GridItemSlot gridItemSlot = new((short)pos.X, (short)pos.Y, this);
            ((Control)GetSlotAt(pos)).GetParent().AddChild(gridItemSlot);
            ((Control)GetSlotAt(pos)).QueueFree();
            _grid[(int)(pos.Y * Columns + pos.X)] = gridItemSlot;
        }
    }

    private bool IsGridPositionValid(Vector2 gridPos)
    {
        if (gridPos.X >= Columns || gridPos.Y >= Rows)
        {
            //GD.Print("hisds");
            return false; // Grid position if not inside the grid
        }

        if (GetSlotAt(gridPos) is GridItemSlotReference)
        {
            return false; // This grid cell is already occupied with another item
        }

        return true;
    }

    private IItemSlot GetSlotAt(Vector2 gridPos)
    {
        return _grid[(int)(gridPos.Y * Columns + gridPos.X)];
    }
}