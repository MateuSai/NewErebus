using Godot;

namespace ErebusInventory;

public interface ItemSlot
{
    void Equip(ItemInfo itemInfo);

    public ItemInfo Grab();

    public ItemInfo GetItemInfo();

    public TextureRect GetIconTextureRect();
}