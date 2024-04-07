using Godot;

namespace ErebusInventory;

public interface IItemSlot
{
    void Equip(ItemInfo itemInfo);

    void Unequip();

    public ItemInfo Grab();

    public ItemInfo GetItemInfo();

    public TextureRect GetIconTextureRect();
}