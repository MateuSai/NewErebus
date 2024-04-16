using Godot;

namespace ErebusInventory;

public interface IItemSlot
{
    bool Equip(ItemInfo itemInfo);

    void Unequip();

    public ItemInfo Grab();

    public ItemInfo GetItemInfo();

    public TextureRect GetIconTextureRect();
}