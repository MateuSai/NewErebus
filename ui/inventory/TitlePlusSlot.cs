using ErebusInventory;
using Godot;
using System;

namespace Erebus.UI.Inventory;

[GlobalClass]
public partial class TitlePlusSlot : VBoxContainer
{
    private Label _titleLabel;
    private EquipmentItemSlot _slot;

    public override void _Ready()
    {
        base._Ready();

        _titleLabel = new()
        {
            ClipContents = true,
            SizeFlagsVertical = SizeFlags.ExpandFill,
            CustomMinimumSize = new(0, 10),
        };
        _titleLabel.AddThemeFontOverride("font", GD.Load<Font>("res://ui/fonts/Wekufupixelsmall.ttf"));
        _titleLabel.AddThemeFontSizeOverride("font_size", 6);
        AddChild(_titleLabel);
        MoveChild(_titleLabel, 0);

        _slot = GetChild<EquipmentItemSlot>(1);
        _slot.ItemInfoChanged += OnItemInfoChanged;
    }

    private void OnItemInfoChanged(ItemInfo itemInfo)
    {
        if (itemInfo == null)
        {
            _titleLabel.Text = "";
        }
        else
        {
            _titleLabel.Text = itemInfo.Id;
        }
    }
}
