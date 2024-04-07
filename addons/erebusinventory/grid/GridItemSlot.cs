using Godot;
using System;

namespace ErebusInventory.Grid;

public partial class GridItemSlot : Control, ITemSlot
{
    private readonly short _x;
    private readonly short _y;

    private ItemInfo _itemInfo;

    public TextureRect Icon;

    private InventorySystem _inventorySystem;

    public GridItemSlot(short x, short y)
    {
        _x = x;
        _y = y;

        Icon = new()
        {
            StretchMode = TextureRect.StretchModeEnum.Keep,
            ZIndex = 1,
        };
        AddChild(Icon);

        SetAnchorsPreset(LayoutPreset.FullRect);
    }

    public override void _Ready()
    {
        base._Ready();

        _inventorySystem = GetNode<InventorySystem>("/root/" + nameof(InventorySystem));

        MouseEntered += () => _inventorySystem.AddSlotUnderMouse(this);
        MouseExited += () => _inventorySystem.RemoveSlotUnderMouse(this);
    }

    public void Equip(ItemInfo itemInfo)
    {
        _itemInfo = itemInfo;

        Icon.Texture = _itemInfo.Icon;
    }

    public TextureRect GetIconTextureRect()
    {
        return (TextureRect)Icon.Duplicate();
    }

    public ItemInfo GetItemInfo()
    {
        return _itemInfo;
    }

    public ItemInfo Grab()
    {
        return _itemInfo;
    }

    public void Unequip()
    {
        Icon.Texture = null;
        _itemInfo = null;
    }
}
