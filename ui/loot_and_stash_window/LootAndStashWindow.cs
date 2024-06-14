using Godot;
using System;

namespace Erebus.UI.LootAndStashWindow;

public partial class LootAndStashWindow : TabContainer
{
    public LootTab LootTab;

    public override void _Ready()
    {
        base._Ready();

        LootTab = GetNode<LootTab>("LOOT");
        SetTabIcon(0, GD.Load<Texture2D>("res://art/ui/Loot_tab_floor_icon.png"));
    }

    public void OnShow()
    {
        LootTab.FillGrid();
    }

    public void OnHide()
    {
        LootTab.ClearGrid();
    }
}
