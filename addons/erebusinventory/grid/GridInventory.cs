using Erebus.Autoloads;
using Erebus.UI.Inventory.DivideStackWindow;
using ErebusLogger;
using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ErebusInventory.Grid;

[GlobalClass, Tool]
public partial class GridInventory : GridContainer
{
    [Export]
    public int Rows;
    [Export]
    private int _cellSize = 16;

    private GridCell[] _grid;

    private GridCell _selectedCell = null;
    private readonly List<GridCell> _selectedCells = new();

    public readonly List<Vector2I> GridsWithItems = new();
    public readonly List<ItemInfo> Items = new();

    public bool CanSplitItems = true;

    private Globals _globals;
    private InventorySystem _inventorySystem;

    public enum InsertResult
    {
        Cancelled,
        Moved,
        Stacked,
        PartlyStacked,
        PartlyMoved,
    }

    public GridInventory()
    {
        AddThemeConstantOverride("h_separation", 0);
        AddThemeConstantOverride("v_separation", 0);
    }

    public override void _Ready()
    {
        base._Ready();

        _globals = GetTree().Root.GetNode<Globals>("Globals");
        _inventorySystem = GetNode<InventorySystem>("/root/" + nameof(InventorySystem));
        _inventorySystem.DraggingItemChanged += (ItemInfo oldItem, ItemInfo newItem) =>
        {
            if (oldItem != null && oldItem.IsConnected(nameof(oldItem.ItemRotated), new Callable(this, nameof(OnItemRotated))))
            {
                oldItem.Disconnect(nameof(oldItem.ItemRotated), new Callable(this, nameof(OnItemRotated)));
            }

            if (newItem != null)
            {
                newItem.ItemRotated += OnItemRotated;
            }

            //Log.Debug("Holy shit");
            RestorePreviouslySelectedCellsColor();
        };

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
                    _selectedCell = gridCell;

                    _selectedCells.Clear();

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
                    _selectedCell = null;

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

    public async void InsertItemAutomatically(ItemInfo itemInfo)
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

        await gridCell.Equip(itemInfo);
    }

    public bool CanInsertItemAt(ItemInfo itemInfo, Vector2I atGridPos)
    {
        if (!IsGridPositionValid(atGridPos, itemInfo))
        {
            Log.Debug("Grid position not valid");
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
            if (!IsGridPositionValid(pos, itemInfo))
            {
                Log.Debug("Grid position not valid");
                return false;
            }
        }

        return true;
    }

    public virtual async Task<InsertResult> InsertItemByDragging(ItemInfo itemInfo, Vector2I atGridPos, bool skipDivideWindow = false)
    {
        Log.Debug("InsertItemByDragging");

        GridCell cell = GetCellAt(atGridPos);

        /*if (!skipDivideWindow && CanSplitItems && Input.IsKeyPressed(Key.Ctrl) && ((cell.GetItemInfo() != null && cell.GetItemInfo().Id == itemInfo.Id && cell.GetItemInfo().IsStackable()) || (cell.GetItemInfo() == null && itemInfo.IsStackable())))
        {
            Log.Debug("Opening window to split item...");
            DivideStackWindow divideStackWindow = GD.Load<PackedScene>("res://ui/inventory/divide_stack_window/DivideStackWindow.tscn").Instantiate<DivideStackWindow>();
            _globals.UI.AddChild(divideStackWindow);
            divideStackWindow.Setup(itemInfo);
            divideStackWindow.Position = GetGlobalMousePosition();
            await ToSignal(divideStackWindow, "tree_exiting");
            Log.Debug("Divide window amount: " + divideStackWindow.GetAmount());
            if (divideStackWindow.GetAmount() == 0)
            {
                Log.Debug("Split cancelled!");
                return InsertResult.Cancelled;
            }
            else
            {
                if (cell.GetItemInfo() == null && itemInfo.IsStackable())
                {
                    Log.Debug("Amount moved to new slot: " + divideStackWindow.GetAmount());
                    ItemInfo itemInfoToInsert = itemInfo.Duplicate();
                    itemInfoToInsert.Amount = divideStackWindow.GetAmount();
                    itemInfo.Amount -= divideStackWindow.GetAmount();
                    IItemSlot.EquipResult equipResult = await cell.Equip(itemInfoToInsert);
                    Log.Debug("Equip new partly moved item result: " + equipResult);
                    return InsertResult.PartlyMoved;
                    //itemInfo = itemInfoToInsert; // So this is the item that is inserted instead of the one we were dragging. It will be inserted on the InsertItem function
                }
                else
                {
                    Log.Debug("Amount to stack: " + divideStackWindow.GetAmount());
                    //if (cell.GetItemInfo() != null && cell.GetItemInfo().Id == itemInfo.Id)
                    //{
                    itemInfo.Amount -= divideStackWindow.GetAmount();
                    cell.GetItemInfo().Amount += divideStackWindow.GetAmount();
                    return InsertResult.PartlyStacked;
                    //}
                }
            }
        }*/

        return InsertItem(itemInfo, atGridPos);
    }

    private InsertResult InsertItem(ItemInfo itemInfo, Vector2I atGridPos)
    {
        if (!CanInsertItemAt(itemInfo, atGridPos))
        {
            Log.Fatal("CanInsertItemAt returns false but called InsertItemByDragging", GetTree());
        }

        GridCell cell = GetCellAt(atGridPos);
        Log.Debug("cell item id: " + (cell.GetItemInfo() != null ? cell.GetItemInfo().Id : "no item on this cell") + "  dragging item id: " + itemInfo.Id);
        Log.Debug("cell item: " + cell.GetItemInfo() + "  dragging item: " + itemInfo);
        if (cell.GetItemInfo() != null && cell.GetItemInfo() != itemInfo && cell.GetItemInfo().Id == itemInfo.Id)
        {
            Log.Debug("Stacking items...");
            cell.GetItemInfo().Amount += itemInfo.Amount;
            itemInfo.Amount = 0;
            return InsertResult.Stacked;
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

        foreach (Vector2I pos in gridPositions)
        {
            Log.Debug("Configuring " + pos + " cell as reference");
            GetCellAt(pos).ConfigureAsCellReference(GetCellAt(atGridPos));
            //GridItemSlotReference gridItemSlotReference = new((GridItemSlot)GetSlotAt(atGridPos));
            //((Control)GetSlotAt(pos)).GetParent().AddChild(gridItemSlotReference);
            //((Control)GetSlotAt(pos)).QueueFree();
            //_grid[(int)(pos.Y * Columns + pos.X)] = gridItemSlotReference;
        }

        GridsWithItems.Add(atGridPos);
        Items.Add(itemInfo);

        return InsertResult.Moved;
    }

    public virtual void RemoveItem(ItemInfo itemInfo, Vector2I atGridPos, int width, int height)
    {
        List<Vector2I> gridPositions = new();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
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

    private bool IsGridPositionValid(Vector2 gridPos, ItemInfo draggingItemInfo)
    {
        GD.Print("Checking grid " + gridPos + " to see if it's valid");

        if (gridPos.X >= Columns || gridPos.Y >= Rows)
        {
            Log.Debug("Gris position outside grid!");
            return false; // Grid position is not inside the grid
        }

        System.Diagnostics.Debug.Assert(GetCellAt(gridPos) != null);
        GridCell cell = GetCellAt(gridPos: gridPos);
        Log.Debug("draggingItemInfo: " + draggingItemInfo);
        Log.Debug("cell ItemInfo: " + cell.GetItemInfo());
        Log.Debug("draggingItemInfo id: " + ((draggingItemInfo != null) ? draggingItemInfo.Id : "NULL"));
        Log.Debug("cell ItemInfo id: " + ((cell.GetItemInfo() != null) ? cell.GetItemInfo().Id : "NULL"));
        Log.Debug("cell is empty: " + cell.IsEmpty().ToString());
        if (!cell.IsEmpty())
        {
            Log.Debug("(draggingItemInfo != null && (cell.GetItemInfo() == draggingItemInfo || (cell.GetItemInfo().Id == draggingItemInfo.Id && cell.GetItemInfo().IsStackable())): " + (draggingItemInfo != null && (cell.GetItemInfo() == draggingItemInfo || (cell.GetItemInfo().Id == draggingItemInfo.Id && cell.GetItemInfo().IsStackable()))).ToString());
        }
        if (cell.IsEmpty() || (draggingItemInfo != null && (cell.GetItemInfo() == draggingItemInfo || (cell.GetItemInfo().Id == draggingItemInfo.Id && cell.GetItemInfo().IsStackable()))))
        {
            Log.Debug("Grid position is valid");
            return true;
        }
        else
        {
            Log.Debug("Grid position is NOT valid");
            return false; // This grid cell is already occupied with another item
        }
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
                if (!IsGridPositionValid(new(gridCell.X, gridCell.Y), _inventorySystem.DraggingItem))
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
        //Log.Debug("RestorePreviouslySelectedCellsColor: " + _selectedCells.Count);
        foreach (GridCell gridCell in _selectedCells)
        {
            gridCell.SetInteriorColor(GridCell.InteriorColor.Default);
        }
    }

    private void OnItemRotated()
    {
        if (_selectedCell != null)
        {
            GridCell cell = _selectedCell;
            cell.EmitSignal("mouse_exited");
            cell.EmitSignal("mouse_entered");
        }
    }
}