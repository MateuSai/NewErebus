using Godot;
using System;

namespace ErebusInventory.Grid;

[Tool]
public partial class GridCell : TextureRect, IItemSlot
{
    public readonly short X;
    public readonly short Y;

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

        X = x;
        Y = y;
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
                _interiorTexture.Modulate = new Color("1d3b1a");
                break;
            case InteriorColor.Red:
                _interiorTexture.Modulate = new Color("5d1a1a");
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
        System.Diagnostics.Debug.Assert(cellToReference != null);

        CellMode = Mode.ItemHolderReference;

        Icon.QueueFree();
        _itemInfo = null;

        _itemHolderReference = cellToReference;
    }

    public bool Equip(ItemInfo itemInfo)
    {
        System.Diagnostics.Debug.Assert(_itemHolderReference == null);

        //GD.Print(_x + " " + _y);

        bool couldInsert = _gridInventory.InsertItem(itemInfo, new Vector2I(X, Y));

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
        GD.Print(X + " " + Y + "     " + (_itemHolderReference != null));
        if (_itemHolderReference == null)
        {
            GD.Print("Returning item info");
            return _itemInfo;
        }
        else
        {
            GD.Print("Redirecting to item holder...");
            return _itemHolderReference.Grab();
        }
    }

    public void Unequip()
    {
        if (_itemHolderReference == null)
        {
            //GD.Print("hey");
            _gridInventory.RemoveItem(_itemInfo, new Vector2I(X, Y));

            Icon.Texture = null;
            _itemInfo = null;
        }
        else
        {
            _itemHolderReference.Unequip();
        }
    }
}
