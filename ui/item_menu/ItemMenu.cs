using ErebusInventory;
using Godot;
using System;
using System.ComponentModel;

namespace Erebus.UI.ItemMenu;

public partial class ItemMenu : PanelContainer
{
    private VBoxContainer _vbox;
    private Button _infoButton;

    public override void _Ready()
    {
        base._Ready();

        _vbox = GetNode<VBoxContainer>("VBoxContainer");
        _infoButton = _vbox.GetNode<Button>("InfoButton");
    }

    public void Initialize(ItemInfo itemInfo)
    {

    }
}
