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

    private GridCell[] _grid;

    private readonly List<GridCell> _selectedCells = new();

    public readonly List<Vector2I> GridsWithItems = new();
    public readonly List<ItemInfo> Items = new();

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

        _grid = new GridCell[Columns * Rows];

        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Columns; j++)
            {
                GridCell gridCell = new((short)j, (short)i, this);
                gridCell.MouseEntered += () =>
                {
                    //GD.Print("hohohoho");
                    if (_inventorySystem.DraggingItem != null)
                    {
                        GD.Print("x: " + gridCell.X + "   y: " + gridCell.Y);
                        //GD.Print("ooooooo");
                        for (int x = 0; x < _inventorySystem.DraggingItem.BaseWidth; x++)
                        {
                            //GD.Print("x: " + x);
                            for (int y = 0; y < _inventorySystem.DraggingItem.BaseHeight; y++)
                            {
                                //GD.Print("y: " + y);
                                if (gridCell.X + x >= Columns || gridCell.Y + y >= Rows)
                                {
                                    continue;
                                }
                                //GD.Print("hey");
                                _selectedCells.Add(GetCellAt(new(gridCell.X + x, gridCell.Y + y)));
                            }
                        }
                        ColorSelectedCells();
                    }
                };
                gridCell.MouseExited += () =>
                {
                    RestorePreviouslySelectedCellsColor();
                    _selectedCells.Clear();
                };
                //GridItemSlot gridItemSlot = new((short)(j % Rows), (short)(j / Columns), this);
                //gridCell.AddChild(gridItemSlot);
                _grid[(int)(i * Columns + j)] = gridCell;
                AddChild(gridCell);
            }
        }
    }

    public void InsertItemAutomatically(ItemInfo itemInfo)
    {
        Vector2I gridPos = new(0, 0);
        GridCell gridCell = null;
        while (true)
        {
            bool suitablePositionFound = true;

            for (int x = 0; x < Columns; x++)
            {
                suitablePositionFound = true;

                GD.Print("Checking column " + x);
                gridPos.X = x;
                for (int widthX = 0; widthX < itemInfo.BaseWidth; widthX++)
                {
                    gridPos.X = x + widthX;
                    GD.Print("Checking " + gridPos);
                    gridCell = GetCellAt(gridPos);
                    if (gridCell == null || !gridCell.IsEmpty())
                    {
                        GD.Print("Cell not available");
                        suitablePositionFound = false;
                        break;
                    }
                }

                if (suitablePositionFound)
                {
                    gridPos.X = x; // So the grid position if at the top left, not the rop right
                    gridCell = GetCellAt(gridPos);
                    break;
                }
            }

            if (suitablePositionFound)
            {
                break;
            }
            else
            {
                gridPos.Y += 1;
            }
        }

        gridCell.Equip(itemInfo);
    }


    public bool InsertItemByDragging(ItemInfo itemInfo, Vector2I atGridPos)
    {
        //GD.Print("Inserting " + itemInfo + " at " + atGridPos);

        if (!IsGridPositionValid(atGridPos))
        {
            return false;
        }

        List<Vector2I> gridPositions = new();

        for (int x = 0; x < itemInfo.BaseWidth; x++)
        {
            for (int y = 0; y < itemInfo.BaseHeight; y++)
            {
                if (x == 0 && y == 0)
                {
                    continue; // We ignore the grid item slot, since we only want to add references to the other occupied grids
                }
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
            GD.Print("Configuring " + pos + " cell as reference");
            GetCellAt(pos).ConfigureAsCellReference(GetCellAt(atGridPos));
            //GridItemSlotReference gridItemSlotReference = new((GridItemSlot)GetSlotAt(atGridPos));
            //((Control)GetSlotAt(pos)).GetParent().AddChild(gridItemSlotReference);
            //((Control)GetSlotAt(pos)).QueueFree();
            //_grid[(int)(pos.Y * Columns + pos.X)] = gridItemSlotReference;
        }

        GridsWithItems.Add(atGridPos);
        Items.Add(itemInfo);

        return true;
    }

    public void RemoveItem(ItemInfo itemInfo, Vector2I atGridPos)
    {
        List<Vector2I> gridPositions = new();

        for (int x = 0; x < itemInfo.BaseWidth; x++)
        {
            for (int y = 0; y < itemInfo.BaseHeight; y++)
            {
                if (x == 0 && y == 0)
                {
                    continue; // We ignore the grid item slot, since we only want to add references to the other occupied grids
                }
                gridPositions.Add(new Vector2I(atGridPos.X + x, atGridPos.Y + y));
            }
        }

        foreach (Vector2I pos in gridPositions)
        {
            GetCellAt(pos).ConfigureAsItemHolder();

            /*  GridItemSlot gridItemSlot = new((short)pos.X, (short)pos.Y, this);
             ((Control)GetSlotAt(pos)).GetParent().AddChild(gridItemSlot);
             ((Control)GetSlotAt(pos)).QueueFree();
             _grid[(int)(pos.Y * Columns + pos.X)] = gridItemSlot; */
        }

        GridsWithItems.Remove(atGridPos);
        Items.Remove(itemInfo);
    }

    private bool IsGridPositionValid(Vector2 gridPos)
    {
        GD.Print("Checking grid " + gridPos + " to see if it's valid");

        if (gridPos.X >= Columns || gridPos.Y >= Rows)
        {
            //GD.Print("hisds");
            return false; // Grid position if not inside the grid
        }

        System.Diagnostics.Debug.Assert(GetCellAt(gridPos) != null);
        if (!GetCellAt(gridPos).IsEmpty())
        {
            return false; // This grid cell is already occupied with another item
        }

        return true;
    }

    public GridCell GetCellAt(Vector2 gridPos)
    {
        return (gridPos.X < Columns && gridPos.Y < Rows) ? _grid[(int)(gridPos.Y * Columns + gridPos.X)] : null;
    }

    private void ColorSelectedCells()
    {
        GridCell.InteriorColor interiorColor = GridCell.InteriorColor.Green;

        if (_selectedCells.Count < (_inventorySystem.DraggingItem.BaseWidth * _inventorySystem.DraggingItem.BaseHeight))
        {
            interiorColor = GridCell.InteriorColor.Red;
        }
        else
        {
            foreach (GridCell gridCell in _selectedCells)
            {
                if (!IsGridPositionValid(new(gridCell.X, gridCell.Y)))
                {
                    interiorColor = GridCell.InteriorColor.Red;
                    break;
                }
            }
        }

        foreach (GridCell gridCell in _selectedCells)
        {
            gridCell.SetInteriorColor(interiorColor);
        }
    }

    private void RestorePreviouslySelectedCellsColor()
    {
        foreach (GridCell gridCell in _selectedCells)
        {
            gridCell.SetInteriorColor(GridCell.InteriorColor.Default);
        }
    }
}