using Godot;

namespace ErebusInventory;

public interface ITemSlot
{
    void Equip(ItemInfo itemInfo);

    void Unequip();

    public ItemInfo Grab();

    public ItemInfo GetItemInfo();

    public TextureRect GetIconTextureRect();
}