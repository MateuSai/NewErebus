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

    protected void AddInventoryGrid(Godot.Collections.Array<Vector2I> inventoryGrid, Node to)
    {
        HBoxContainer hBox = new()
        {
            Alignment = BoxContainer.AlignmentMode.Center,
        };
        hBox.AddThemeConstantOverride("separation", 3);

        foreach (Vector2I grid in inventoryGrid)
        {
            MarginContainer container = new()
            {
                SizeFlagsHorizontal = SizeFlags.ShrinkCenter,
                SizeFlagsVertical = SizeFlags.ShrinkCenter,
            };

            MarginContainer gridMarginContainer = new();
            gridMarginContainer.AddThemeConstantOverride("margin_left", 2);
            gridMarginContainer.AddThemeConstantOverride("margin_top", 2);
            gridMarginContainer.AddThemeConstantOverride("margin_right", 2);
            gridMarginContainer.AddThemeConstantOverride("margin_bottom", 2);

            GridInventory gridInventory = new()
            {
                Columns = grid.X,
                Rows = grid.Y
            };
            gridMarginContainer.AddChild(node: gridInventory);
            container.AddChild(gridMarginContainer);

            NinePatchRect frameNinePatchRect = new()
            {
                Texture = GD.Load<Texture2D>("res://art/ui/Inventory_grid_frame.png"),
                PatchMarginLeft = 5,
                PatchMarginTop = 5,
                PatchMarginRight = 5,
                PatchMarginBottom = 5,
            };
            frameNinePatchRect.SetAnchorsPreset(LayoutPreset.FullRect);
            container.AddChild(frameNinePatchRect);

            hBox.AddChild(container);
        }

        to.CallDeferred("add_child", hBox);
    }

    protected virtual void OnItemInfoChanged(ItemInfo itemInfo)
    {
    }
}
