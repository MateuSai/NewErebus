using Godot;
using System;

namespace Erebus.UI.LootAndStashWindow;

public partial class LootAndStashWindow : TabContainer
{
    private LootTab _lootTab;

    public override void _Ready()
    {
        base._Ready();

        _lootTab = GetNode<LootTab>("LOOT");
        SetTabIcon(0, GD.Load<Texture2D>("res://art/ui/Loot_tab_floor_icon.png"));
    }

    public void OnShow()
    {
        _lootTab.FillGrid();
    }

    public void OnHide()
    {
        _lootTab.ClearGrid();
    }
}
