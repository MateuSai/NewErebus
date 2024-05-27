using System.Threading.Tasks;
using Godot;

namespace ErebusInventory;

public interface IItemSlot
{
    enum EquipResult
    {
        Moved,
        Stacked,
        Cancelled,
    }

    bool CanEquip(ItemInfo itemInfo);
    /// <summary>
    /// Equips item to the slot. Dependending on the situation, the item can be moved from one slot to the other, stacked...
    /// </summary>
    Task<EquipResult> Equip(ItemInfo itemInfo);

    void Unequip();

    public ItemInfo Grab();

    public ItemInfo GetItemInfo();

    public TextureRect GetIconTextureRect();
}