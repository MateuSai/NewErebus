using Erebus.UI.Inventory;
using Erebus.UI.ItemMenu;
using ErebusLogger;
using Godot;
using System;
using System.Threading.Tasks;
using Log = ErebusLogger.Log;

namespace ErebusInventory;

[Tool, GlobalClass]
public partial class EquipmentItemSlot : CenterContainer, IItemSlot
{
    private ItemInfo _itemInfo;
    private void SetItemInfo(ItemInfo itemInfo)
    {
        _itemInfo = itemInfo;
        EmitSignal(nameof(ItemInfoChanged), itemInfo);
    }


    private InventorySystem _inventorySystem;

    protected TextureRect _emptyTextureRect;
    private TextureRect _icon;

    [Signal]
    public delegate void ItemInfoChangedEventHandler(ItemInfo itemInfo);

    public EquipmentItemSlot()
    {
        _emptyTextureRect = new()
        {
            Texture = GD.Load<Texture2D>("res://art/ui/Equip_tab_misc_empty.png")
        };
        AddChild(_emptyTextureRect);
        _emptyTextureRect.Hide();

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

    public virtual bool CanEquip(ItemInfo itemInfo)
    {
        return true;
    }

    public virtual void Equip(ItemInfo itemInfo)
    {
        Log.Debug("Equip");
        if (!CanEquip(itemInfo))
        {
            Log.Fatal("Tried to equip item on slot when CanEquip returns false", GetTree());
        }

        SetItemInfo(itemInfo);

        _emptyTextureRect.Show();
        _icon.Texture = _itemInfo.Icon;

        return; //Task.FromResult(IItemSlot.EquipResult.Moved);
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
        _emptyTextureRect.Hide();
        _icon.Texture = null;
        SetItemInfo(null);
    }

    protected virtual void OnItemInfoChanged(ItemInfo itemInfo)
    {
    }

    public ItemMenu OpenItemMenu()
    {
        throw new NotImplementedException();
    }
}
