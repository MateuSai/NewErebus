using Godot;

namespace ErebusInventory;

public interface IItemSlot
{
    bool CanEquip(ItemInfo itemInfo);
    void Equip(ItemInfo itemInfo);

    void Unequip();

    public ItemInfo Grab();

    public ItemInfo GetItemInfo();

    public TextureRect GetIconTextureRect();
}