using Erebus.UI.Inventory;
using ErebusInventory;
using ErebusInventory.Grid;
using ErebusLogger;
using Godot;
using System;
using System.Diagnostics;
using System.Linq;

namespace Erebus.UI.InventoryWindow;

public partial class InventoryWindow : VBoxContainer
{
    public enum Type
    {
        Torso,
        Legs,
        Backpack
    }

    private UI _ui;

    private MarginContainer _torsoInventory;
    private MarginContainer _legsInventory;
    private MarginContainer _backpackInventory;

    public override void _EnterTree()
    {
        base._EnterTree();

        _ui = GetTree().CurrentScene.GetNode<UI>("UI");

        _torsoInventory = GetNode<MarginContainer>("TorsoInventory");
        _legsInventory = GetNode<MarginContainer>("LegsInventory");
        _backpackInventory = GetNode<MarginContainer>("BackpackInventory");

        _torsoInventory.Hide();
        _legsInventory.Hide();
        _backpackInventory.Hide();
    }

    public void AddInventory(BodyEquipment itemInfo, Type type)
    {
        MarginContainer[] inventoryMarginContainers = { _torsoInventory, _legsInventory, _backpackInventory };
        MarginContainer inventoryMarginContainer = inventoryMarginContainers[(int)type];

        Debug.Assert(!inventoryMarginContainer.Visible);

        inventoryMarginContainer.GetNode<Label>("VBoxContainer/TextureRect/Label").Text = itemInfo.Id.ToSnakeCase().ToUpper();

        HBoxContainer _gridInventoryHBox = new()
        {
            Alignment = BoxContainer.AlignmentMode.Center,
            Name = "HBox",
        };
        _gridInventoryHBox.AddThemeConstantOverride("separation", 3);

        foreach (Vector2I grid in itemInfo.InventoryGrid)
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
                Rows = grid.Y,
                BodyEquipmentThatGeneratedGrid = itemInfo,
            };
            gridMarginContainer.AddChild(node: gridInventory);
            //_gridInventories.Add(gridInventory);

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

        inventoryMarginContainer.GetNode("VBoxContainer/MarginContainer/MarginContainer").CallDeferred("add_child", _gridInventoryHBox);

        inventoryMarginContainer.Show();
    }

    public void RemoveInventoryGrid(Type type)
    {
        MarginContainer[] inventoryMarginContainers = { _torsoInventory, _legsInventory, _backpackInventory };
        MarginContainer inventoryMarginContainer = inventoryMarginContainers[(int)type];

        if (!inventoryMarginContainer.Visible)
        {
            return; // The item unequipped did not have inventory
        }

        var inventoriesContainer = inventoryMarginContainer.GetNode("VBoxContainer/MarginContainer/MarginContainer/HBox");

        foreach (Node child in inventoriesContainer.GetChildren())
        {
            GridInventory gridInventory = child.GetChild(0).GetChild<GridInventory>(0);

            Log.Info("RemoveInventoryGrid: " + gridInventory.Items.Count + " on grid");
            for (int i = gridInventory.Items.Count - 1; i > -1; i--)
            {
                ItemInfo item = gridInventory.Items[i];
                _ui.LootAndStashWindow.LootTab.Grid.InsertItemAutomatically(item);
                //_ui.LootAndStashWindow.LootTab.Grid.SpawnItemOnFlor(item);
                /* ItemOnFloor itemOnFloor = new()
                {
                    ItemInfo = _gridInventory.Items[i],
                    Position = GetTree().Root.GetNode<Globals>("Globals").Player.Position
                };
                GetTree().CurrentScene.AddChild(itemOnFloor); */
            }
        }

        //_gridInventoryHBox.QueueFree();
        //_gridInventoryHBox = null;
        //_gridInventories.Clear();

        inventoryMarginContainer.Hide();
    }
}
