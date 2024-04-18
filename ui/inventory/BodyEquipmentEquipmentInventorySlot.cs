using ErebusInventory;
using ErebusInventory.Grid;
using Godot;
using System;
using System.Collections.Generic;

namespace Erebus.UI.Inventory;

public partial class BodyEquipmentEquipmentInventorySlot : EquipmentItemSlot
{
    public override void _Ready()
    {
        base._Ready();

        ItemInfoChanged += OnItemInfoChanged;
    }

    protected void AddInventoryGrid(List<Vector2I> inventoryGrid, Node to)
    {
        HBoxContainer hBox = new()
        {
            Alignment = BoxContainer.AlignmentMode.Center
        };

        foreach (Vector2I grid in inventoryGrid)
        {
            GridInventory gridInventory = new()
            {
                Columns = grid.X,
                Rows = grid.Y
            };
            hBox.AddChild(gridInventory);
        }

        to.CallDeferred("add_child", hBox);
    }

    protected virtual void OnItemInfoChanged(ItemInfo itemInfo)
    {
    }
}
