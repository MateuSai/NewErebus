using Godot;
using System;

namespace ErebusInventory.Grid;

[Tool]
public partial class GridCell : TextureRect
{
    public GridCell()
    {
        Texture = GD.Load<Texture2D>("res://art/ui/Inventory_grid_cell.png");
    }
}
