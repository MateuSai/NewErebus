using ErebusInventory;
using ErebusInventory.Grid;
using Godot;
using System;
using System.Collections.Generic;

namespace Erebus.UI.Inventory;

public partial class BodyEquipmentEquipmentInventorySlot : EquipmentItemSlot
{
    private VBoxContainer _inventoryWindow;

    public override void _Ready()
    {
        base._Ready();

        _inventoryWindow = GetOwner<VBoxContainer>().GetNode<VBoxContainer>("../InventoryWindow");

        ItemInfoChanged += OnItemInfoChanged;
    }

    private void AddInventoryGrid(List<Vector2I> inventoryGrid)
    {
        HBoxContainer hBox = new();

        foreach (Vector2I grid in inventoryGrid)
        {
            GridInventory gridInventory = new()
            {
                Columns = grid.X,
                Rows = grid.Y
            };
            hBox.AddChild(gridInventory);
        }

        _inventoryWindow.CallDeferred("add_child", hBox);
    }

    protected virtual void OnItemInfoChanged(ItemInfo itemInfo)
    {
        BodyEquipment bodyEquipment = (BodyEquipment)itemInfo;

        if (bodyEquipment.InventoryGrid != null)
        {
            AddInventoryGrid(bodyEquipment.InventoryGrid);
        }
    }
}
