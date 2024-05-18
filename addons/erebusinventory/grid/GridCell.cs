using ErebusLogger;
using Godot;
using System;
using System.Diagnostics;

namespace ErebusInventory.Grid;

[Tool]
public partial class GridCell : TextureRect, IItemSlot
{
    public readonly short X;
    public readonly short Y;

    private ItemInfo _itemInfo;
    private int _width;
    private int _height;

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
    private NinePatchRect _itemBackgroundPanel;
    private TextureRect _interiorTexture;
    private Label _label;

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

        _itemBackgroundPanel = new()
        {
            Texture = GD.Load<Texture2D>("res://art/ui/Inventory_slot_2x2.png"),
            PatchMarginLeft = 2,
            PatchMarginTop = 2,
            PatchMarginRight = 2,
            PatchMarginBottom = 2,
            ZIndex = 1,

        };
        AddChild(_itemBackgroundPanel);
        _itemBackgroundPanel.Hide();

        _label = new()
        {
            ZIndex = 3,
            HorizontalAlignment = HorizontalAlignment.Right,
            VerticalAlignment = VerticalAlignment.Bottom,
            CustomMinimumSize = Vector2.One * 14,
        };
        _label.AddThemeColorOverride("font_outline_color", Colors.Black);
        _label.AddThemeConstantOverride("outline_size", 1);
        _label.AddThemeFontOverride("font", GD.Load<Font>("res://ui/fonts/Wekufupixelsmall.ttf"));
        _label.AddThemeFontSizeOverride("font_size", 6);
        AddChild(_label);

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

        if (!IsEmpty())
        {
            if (CellMode == Mode.ItemHolderReference)
            {
                _itemHolderReference.SetInteriorColor(interiorColor);
            }
            else
            {
                switch (interiorColor)
                {
                    case InteriorColor.Default:
                        Icon.Modulate = Colors.White;
                        break;
                    case InteriorColor.Green:
                        Icon.Modulate = new Color("1d3b1a");
                        break;
                    case InteriorColor.Red:
                        Icon.Modulate = new Color("5d1a1a");
                        break;
                    default:
                        System.Diagnostics.Debug.Assert(false, "Invalid interior color value");
                        break;
                }
            }
        }
    }

    public void ConfigureAsItemHolder()
    {
        CellMode = Mode.ItemHolder;

        _itemHolderReference = null;

        Icon = new()
        {
            StretchMode = StretchModeEnum.Keep,
            ZIndex = 2,
            MouseFilter = MouseFilterEnum.Ignore,
        };
        AddChild(Icon);
    }
    public void ConfigureAsCellReference(GridCell cellToReference)
    {
        System.Diagnostics.Debug.Assert(cellToReference != null);

        CellMode = Mode.ItemHolderReference;

        Icon.QueueFree();
        SetItemInfo(null);

        _itemHolderReference = cellToReference;
    }

    public bool CanEquip(ItemInfo itemInfo)
    {
        bool couldInsert = _gridInventory.CanInsertItemAt(itemInfo, new Vector2I(X, Y));

        if (!couldInsert)
        {
            return false;
        }

        return true;
    }

    public async void Equip(ItemInfo itemInfo)
    {
        if (!CanEquip(itemInfo))
        {
            Log.Fatal("Tried to equip item on cell when CanEquip returns false", GetTree());
        }

        GridInventory.InsertResult res = await _gridInventory.InsertItemByDragging(itemInfo, new Vector2I(X, Y));
        if (res == GridInventory.InsertResult.Stacked || res == GridInventory.InsertResult.Cancelled)
        {
            Debug.Assert(GetItemInfo() != null);
            return;
        }

        SetItemInfo(itemInfo);
        //_itemInfo = itemInfo;
        _width = _itemInfo.BaseWidth;
        _height = _itemInfo.BaseHeight;

        Icon.Texture = _itemInfo.Icon;
        Icon.PivotOffset = Vector2.One * Icon.Texture.GetSize().X / 2;
        Icon.Rotation = (float)(_itemInfo.Rotated ? -Math.PI / 2.0 : 0.0);

        _itemBackgroundPanel.Size = new Vector2(16 * itemInfo.BaseWidth, 16 * itemInfo.BaseHeight);
        _itemBackgroundPanel.Show();

        if (_itemInfo.IsStackable())
        {
            _label.Position = _itemBackgroundPanel.Size - Vector2.One * 16;
            _label.Show();
        }
        else
        {
            _label.Hide();
        }
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

    private void SetItemInfo(ItemInfo newItemInfo)
    {
        if (_itemInfo != null)
        {
            _itemInfo.AmountChanged -= OnItemAmountChanged;
            _label.Text = "";
        }
        _itemInfo = newItemInfo;
        if (_itemInfo != null)
        {
            _itemInfo.AmountChanged += OnItemAmountChanged;
            OnItemAmountChanged(_itemInfo.Amount);
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
            _gridInventory.RemoveItem(_itemInfo, new Vector2I(X, Y), _width, _height);

            Icon.Texture = null;
            SetItemInfo(null);
            _itemBackgroundPanel.Hide();
        }
        else
        {
            _itemHolderReference.Unequip();
        }

        SetInteriorColor(InteriorColor.Default);
    }

    public bool IsEmpty()
    {
        return _itemInfo == null && CellMode == Mode.ItemHolder;
    }

    private void OnItemAmountChanged(int newAmount)
    {
        _label.Text = newAmount.ToString();
    }
}
