using Godot;
using System;

namespace ErebusInventory.Grid;

[Tool]
public partial class GridCell : TextureRect, IItemSlot
{
    private readonly short _x;
    private readonly short _y;

    private ItemInfo _itemInfo;

    public TextureRect Icon;
    private GridCell _itemHolderReference;

    public enum Mode
    {
        ItemHolder,
        ItemHolderReference,
    }
    public Mode CellMode = Mode.ItemHolder;


    public enum InteriorColor
    {
        Default,
        Green,
        Red,
    }

    private InventorySystem _inventorySystem;
    private GridInventory _gridInventory;
    private TextureRect _interiorTexture;

    public GridCell(short x, short y, GridInventory gridInventory)
    {
        Texture = GD.Load<Texture2D>("res://art/ui/Inventory_grid_cell_exterior_white.png");
        StretchMode = StretchModeEnum.Keep;
        Size = Vector2.Zero;

        _interiorTexture = new()
        {
            Texture = GD.Load<Texture2D>("res://art/ui/Inventory_grid_cell_interior_white.png"),
            StretchMode = StretchModeEnum.Keep,
            Size = Vector2.Zero,
            Position = Vector2.One
        };
        AddChild(_interiorTexture);

        SetInteriorColor(InteriorColor.Default);

        _x = x;
        _y = y;
        _gridInventory = gridInventory;
    }

    public override void _Ready()
    {
        base._Ready();

        _inventorySystem = GetNode<InventorySystem>("/root/" + nameof(InventorySystem));

        ConfigureAsItemHolder();

        MouseEntered += () => _inventorySystem.AddSlotUnderMouse(this);
        MouseExited += () => _inventorySystem.RemoveSlotUnderMouse(this);
    }

    public void SetInteriorColor(InteriorColor interiorColor)
    {
        switch (interiorColor)
        {
            case InteriorColor.Default:
                _interiorTexture.Modulate = new Color("1d1a1a");
                break;
            case InteriorColor.Green:
                _interiorTexture.Modulate = Colors.Green;
                break;
            case InteriorColor.Red:
                _interiorTexture.Modulate = Colors.Red;
                break;
            default:
                System.Diagnostics.Debug.Assert(false, "Invalid interior color value");
                break;
        }
    }

    public void ConfigureAsItemHolder()
    {
        CellMode = Mode.ItemHolder;

        _itemHolderReference = null;

        Icon = new()
        {
            StretchMode = TextureRect.StretchModeEnum.Keep,
            ZIndex = 1,
            MouseFilter = MouseFilterEnum.Ignore,
        };
        AddChild(Icon);
    }
    public void ConfigureAsCellReference(GridCell cellToReference)
    {
        CellMode = Mode.ItemHolderReference;

        Icon.QueueFree();
        _itemInfo = null;

        _itemHolderReference = cellToReference;
    }

    public bool Equip(ItemInfo itemInfo)
    {
        System.Diagnostics.Debug.Assert(_itemHolderReference == null);

        //GD.Print(_x + " " + _y);

        bool couldInsert = _gridInventory.InsertItem(itemInfo, new Vector2I(_x, _y));

        if (!couldInsert)
        {
            return false;
        }

        _itemInfo = itemInfo;

        Icon.Texture = _itemInfo.Icon;

        return true;
    }

    public TextureRect GetIconTextureRect()
    {
        if (_itemHolderReference == null)
        {
            return (TextureRect)Icon.Duplicate();
        }
        else
        {
            return _itemHolderReference.GetIconTextureRect();
        }
    }

    public ItemInfo GetItemInfo()
    {
        if (_itemHolderReference == null)
        {
            return _itemInfo;
        }
        else
        {
            return _itemHolderReference.GetItemInfo();
        }
    }

    public ItemInfo Grab()
    {
        GD.Print(_x + " " + _y);
        if (_itemHolderReference == null)
        {
            return _itemInfo;
        }
        else
        {
            return _itemHolderReference.Grab();
        }
    }

    public void Unequip()
    {
        if (_itemHolderReference == null)
        {
            //GD.Print("hey");
            _gridInventory.RemoveItem(_itemInfo, new Vector2I(_x, _y));

            Icon.Texture = null;
            _itemInfo = null;
        }
        else
        {
            _itemHolderReference.Unequip();
        }
    }
}
