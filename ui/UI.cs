using Erebus.Autoloads;
using Erebus.UI.LootAndStashWindow;
using Godot;
using System;

namespace Erebus.UI;

public partial class UI : CanvasLayer
{
    private bool _inventoriesOpen = false;

    private Globals _globals;
    private ColorRect _darkBackground;
    private EquipWindow.EquipWindow _equipWindow;
    private VBoxContainer _inventoryWindow;
    public LootAndStashWindow.LootAndStashWindow LootAndStashWindow;

    public override void _Ready()
    {
        base._Ready();

        _globals = GetTree().Root.GetNode<Globals>("Globals");
        _darkBackground = GetNode<ColorRect>("DarkBackground");
        _equipWindow = GetNode<EquipWindow.EquipWindow>("%EquipWindow");
        _inventoryWindow = GetNode<VBoxContainer>("%InventoryWindow");
        LootAndStashWindow = GetNode<LootAndStashWindow.LootAndStashWindow>("%LootAndStashWindow");

        HideInventories();
        _darkBackground.Hide();

        _globals.UI = this;
    }

    public override void _ExitTree()
    {
        base._ExitTree();

        _globals.UI = null;
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        base._UnhandledInput(@event);

        if (@event.IsActionPressed("inventory"))
        {
            if (_inventoriesOpen)
            {
                HideInventories();
            }
            else
            {
                ShowInventories();
            }
            //_equipWindow.Visible = !_equipWindow.Visible;
            //_inventoryWindow.Visible = !_inventoryWindow.Visible;
            //_lootAndStashWindow.Visible = !_lootAndStashWindow.Visible;
        }
    }

    private void ShowInventories()
    {
        _darkBackground.Show();

        _inventoriesOpen = true;
        _equipWindow.Show();
        _inventoryWindow.Show();
        LootAndStashWindow.Show();
        LootAndStashWindow.OnShow();
    }

    private void HideInventories()
    {
        _darkBackground.Hide();

        _inventoriesOpen = false;
        _equipWindow.Hide();
        _inventoryWindow.Hide();
        LootAndStashWindow.Hide();
        LootAndStashWindow.OnHide();
    }

    public void MoveDarkBackground(int zIndex)
    {
        _darkBackground.ZIndex = zIndex;
    }
}
