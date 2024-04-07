using Godot;
using System;

namespace ErebusInventory.Grid;

[GlobalClass, Tool]
public partial class GridInventory : GridContainer
{
    [Export]
    private int _rows;
    public int Rows
    {
        get => _rows;
        set
        {
            if (IsInsideTree())
            {
                CreateGrid();
            }
        }
    }
    [Export]
    private int _cellSize = 16;

    private GridItemSlot[] _grid;

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

        _grid = new GridItemSlot[Columns * _rows];

        for (int j = 0; j < _rows * Columns; j++)
        {
            GridCell gridCell = new();

            GridItemSlot gridItemSlot = new((short)(j % _rows), (short)(j / Columns), this);
            gridCell.AddChild(gridItemSlot);
            _grid[j] = gridItemSlot;

            AddChild(gridCell);
        }
    }

    /*public bool InsertItem(ItemInfo itemInfo, Vector2I atGridPos)
    {
        TextureRect icon = new()
        {
            Texture = itemInfo.Icon,
            Position = GridToPos(atGridPos)
        };
        AddChild(icon);

        ItemInfoGridSlot itemInfoGridSlot = new((short)atGridPos.X, (short)atGridPos.Y, itemInfo, icon);
        _grid[atGridPos.X * atGridPos.Y] = itemInfoGridSlot;
        for (int i = 1; i < itemInfo.BaseWidth; i++)
        {
            _grid[atGridPos.X + i, atGridPos.Y] = new GridSlotReference(itemInfoGridSlot);
        }
        for (int i = 1; i < itemInfo.BaseHeight; i++)
        {
            _grid[atGridPos.X, atGridPos.Y + i] = new GridSlotReference(itemInfoGridSlot);
        }

        return true;
    }*/

    private Vector2I PosToGrid(Vector2 pos)
    {
        return new Vector2I((int)(pos.X / _cellSize), (int)(pos.Y / _cellSize));
    }

    private Vector2I GridToPos(Vector2I gridPos)
    {
        return gridPos * _cellSize;
    }
}