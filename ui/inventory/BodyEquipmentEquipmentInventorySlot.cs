using Erebus.Autoloads;
using Erebus.Items;
using ErebusInventory;
using ErebusInventory.Grid;
using ErebusLogger;
using Godot;
using System;
using System.Collections.Generic;

namespace Erebus.UI.Inventory;

public partial class BodyEquipmentEquipmentInventorySlot : EquipmentItemSlot
{
    private UI _ui;

    private HBoxContainer _gridInventoryHBox = null;
    private List<GridInventory> _gridInventories = new();

    public override void _Ready()
    {
        base._Ready();

        _ui = GetTree().CurrentScene.GetNode<UI>("UI");

        ItemInfoChanged += OnItemInfoChanged;
    }

    protected void AddInventoryGrid(Godot.Collections.Array<Vector2I> inventoryGrid, Node to)
    {
        _gridInventoryHBox = new()
        {
            Alignment = BoxContainer.AlignmentMode.Center,
        };
        _gridInventoryHBox.AddThemeConstantOverride("separation", 3);

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
            _gridInventories.Add(gridInventory);

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

            _gridInventoryHBox.AddChild(container);
        }

        to.CallDeferred("add_child", _gridInventoryHBox);
    }

    protected void RemoveInventoryGrid()
    {
        foreach (GridInventory gridInventory in _gridInventories)
        {
            Log.Info("RemoveInventoryGrid: " + gridInventory.Items.Count + " on grid");
            for (int i = gridInventory.Items.Count - 1; i > -1; i--)
            {
                ItemInfo item = gridInventory.Items[i];
                _ui.LootAndStashWindow.LootTab.Grid.InsertItemAutomatically(item);
                _ui.LootAndStashWindow.LootTab.Grid.SpawnItemOnFlor(item);
                /* ItemOnFloor itemOnFloor = new()
                {
                    ItemInfo = _gridInventory.Items[i],
                    Position = GetTree().Root.GetNode<Globals>("Globals").Player.Position
                };
                GetTree().CurrentScene.AddChild(itemOnFloor); */
            }
        }

        _gridInventoryHBox.QueueFree();
        _gridInventoryHBox = null;
        _gridInventories.Clear();
    }

    protected override void OnItemInfoChanged(ItemInfo itemInfo)
    {
        base.OnItemInfoChanged(itemInfo);

        if (itemInfo != null)
        {
            BodyEquipment bodyEquipment = (BodyEquipment)itemInfo;

            if (bodyEquipment.InventoryGrid != null)
            {
                AddInventoryGrid(bodyEquipment.InventoryGrid, GetOwner<Control>().GetNode("../InventoryWindow/BackpackInventory/VBoxContainer/MarginContainer/MarginContainer"));
            }
        }
        else
        {
            if (_gridInventoryHBox != null)
            {
                RemoveInventoryGrid();
            }
        }
    }
}
