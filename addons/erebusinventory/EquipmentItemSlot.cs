using Erebus.UI.Inventory;
using Godot;
using System;

namespace ErebusInventory;

[Tool, GlobalClass]
public partial class EquipmentItemSlot : CenterContainer, IItemSlot
{
    private ItemInfo _itemInfo;

    private InventorySystem _inventorySystem;

    private TextureRect _icon;

    [Signal]
    public delegate void ItemEquippedEventHandler(ItemInfo itemInfo);

    public EquipmentItemSlot()
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
    }

    public virtual bool Equip(ItemInfo itemInfo)
    {
        GD.Print("Equip");
        _itemInfo = itemInfo;

        _icon.Texture = _itemInfo.Icon;

        EmitSignal(nameof(ItemEquipped), _itemInfo);
        return true;
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

    public void Unequip()
    {
        _icon.Texture = null;
        _itemInfo = null;
    }
}
