using Godot;
using System;

namespace ErebusInventory;

[Tool, GlobalClass]
public partial class ItemSlot : CenterContainer
{
    private ItemInfo _itemInfo;

    private InventorySystem _inventorySystem;

    private TextureRect _icon;

    public ItemSlot()
    {
        _icon = new()
        {
            MouseFilter = MouseFilterEnum.Ignore
        };
        AddChild(_icon);
    }

    public override void _Ready()
    {
        base._Ready();

        _inventorySystem = GetNode<InventorySystem>("/root/" + nameof(InventorySystem));

        MouseEntered += () => _inventorySystem.AddSlotUnderMouse(this);
        MouseExited += () => _inventorySystem.RemoveSlotUnderMouse(this);

        Equip(new ItemInfo("id", GD.Load<Texture2D>("res://icon.svg"), 1, 1));
    }

    public void Equip(ItemInfo itemInfo)
    {
        _itemInfo = itemInfo;

        _icon.Texture = _itemInfo.Icon;
    }

    public ItemInfo Grab()
    {
        return _itemInfo;
    }

    public ItemInfo GetItemInfo()
    {
        return _itemInfo;
    }

    public TextureRect GetIconTextureRect()
    {
        return (TextureRect)_icon.Duplicate();
    }
}
