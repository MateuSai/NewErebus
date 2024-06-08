using Erebus.Autoloads;
using Erebus.UI.Inventory.DivideStackWindow;
using Erebus.UI.ItemMenu;
using ErebusLogger;
using Godot;
using System;
using System.Collections.Generic;
using Log = ErebusLogger.Log;

namespace ErebusInventory;

public partial class InventorySystem : CanvasLayer
{
    private IItemSlot _draggingItemSlot = null;
    [Signal]
    public delegate void DraggingItemChangedEventHandler(ItemInfo oldItem, ItemInfo newItem);
    private ItemInfo _draggingItemInfo = null;
    public ItemInfo DraggingItem
    {
        get => _draggingItemInfo;
        set
        {
            Log.Debug("Set DraggingItem");
            EmitSignal(nameof(DraggingItemChanged), _draggingItemInfo, value);
            _draggingItemInfo = value;
        }
    }
    private TextureRect _draggingIcon = null;

    private ItemMenu _currentItemMenu = null;

    private Globals _globals;

    private readonly List<IItemSlot> _slotsUnderMouse = new();

    public InventorySystem()
    {
        Layer = 2; // So the dragging texture appears on top of canvas layers with layer 1
    }

    public override void _Ready()
    {
        base._Ready();

        _globals = GetTree().Root.GetNode<Globals>("Globals");

        SetProcess(false);
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);

        if (@event.IsActionPressed("grab") && _draggingIcon == null)
        {
            GD.Print("Grab");

            if (_currentItemMenu != null)
            {
                _currentItemMenu.QueueFree();
                _currentItemMenu = null;
            }

            if (Grab())
            {
                SetProcess(true);
            }
        }
        else if (@event.IsActionReleased("grab") && _draggingIcon != null)
        {
            GD.Print("release");
            SetProcess(false);
            Release();
        }

        if (@event is InputEventKey key && key.Keycode == Key.R && key.IsPressed() && _draggingItemInfo != null && _draggingItemInfo.BaseWidth != _draggingItemInfo.BaseHeight)
        {
            _draggingItemInfo.Rotated = !_draggingItemInfo.Rotated;
            _draggingIcon.PivotOffset = Vector2.One * _draggingIcon.Texture.GetSize().X / 2;
            _draggingIcon.Rotation = (float)(_draggingItemInfo.Rotated ? -Math.PI / 2.0 : 0.0);
        }

        if (@event.IsActionPressed("ui_item_menu") && _draggingIcon == null)
        {
            if (_slotsUnderMouse.Count > 0 && _slotsUnderMouse[0].GetItemInfo() != null)
            {
                if (_currentItemMenu != null)
                {
                    _currentItemMenu.QueueFree();
                    _currentItemMenu = null;
                }

                _currentItemMenu = _slotsUnderMouse[0].OpenItemMenu();
            }
        }

    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        System.Diagnostics.Debug.Assert(_draggingIcon != null);
        _draggingIcon.Position = GetViewport().GetMousePosition();
    }

    private bool Grab()
    {
        if (_slotsUnderMouse.Count == 0)
        {
            GD.Print("No slots under mouse, cannot grab");
            return false;
        }

        _draggingItemSlot = _slotsUnderMouse[0];
        DraggingItem = _draggingItemSlot.Grab();
        if (DraggingItem == null)
        {
            _draggingItemSlot = null;
            GD.Print("Empty slot, cannot grab");
            return false;
        }

        _draggingIcon = _draggingItemSlot.GetIconTextureRect();
        _draggingIcon.MouseFilter = Control.MouseFilterEnum.Ignore;
        //_draggingIcon.ZIndex += 50;
        AddChild(_draggingIcon);

        return true;
    }

    private async void Release()
    {
        Disable();

        //GD.Print("Slots under mouse: " + _slotsUnderMouse.Count);
        if (_slotsUnderMouse.Count == 0)
        {
            TweenDraggingIconBack();
        }
        else
        {
            IItemSlot slot = _slotsUnderMouse[0];

            if (slot.CanEquip(DraggingItem))
            {
                bool cancelledDivide = false;

                if (Input.IsKeyPressed(Key.Ctrl) && DraggingItem.IsStackable())
                {
                    Log.Debug("Opening window to split item...");
                    DivideStackWindow divideStackWindow = GD.Load<PackedScene>("res://ui/inventory/divide_stack_window/DivideStackWindow.tscn").Instantiate<DivideStackWindow>();
                    _globals.UI.AddChild(divideStackWindow);
                    divideStackWindow.Setup(DraggingItem);
                    divideStackWindow.Position = GetViewport().GetMousePosition();
                    _draggingIcon.Hide();
                    await ToSignal(divideStackWindow, "tree_exiting");

                    if (divideStackWindow.GetAmount() == 0)
                    {
                        Log.Debug("Split cancelled!");
                        cancelledDivide = true;
                    }
                    else if (divideStackWindow.GetAmount() == DraggingItem.Amount)
                    {
                        // We do nothing, the item will me moved as if no split window was open
                    }
                    else
                    {
                        if (slot.GetItemInfo() == null && DraggingItem.IsStackable())
                        {
                            Log.Debug("Amount moved to new slot: " + divideStackWindow.GetAmount());
                            ItemInfo itemInfoToInsert = (ItemInfo)DraggingItem.Duplicate();
                            itemInfoToInsert.Amount = divideStackWindow.GetAmount();
                            DraggingItem.Amount -= divideStackWindow.GetAmount();
                            DraggingItem = itemInfoToInsert;
                            //IItemSlot.EquipResult equipResult = await slot.Equip(itemInfoToInsert);
                            //Log.Debug("Equip new partly moved item result: " + equipResult);
                            // return InsertResult.PartlyMoved;
                            //itemInfo = itemInfoToInsert; // So this is the item that is inserted instead of the one we were dragging. It will be inserted on the InsertItem function
                        }
                        else
                        {
                            Log.Debug("Amount to stack: " + divideStackWindow.GetAmount());
                            //if (cell.GetItemInfo() != null && cell.GetItemInfo().Id == itemInfo.Id)
                            //{
                            DraggingItem.Amount -= divideStackWindow.GetAmount();
                            slot.GetItemInfo().Amount += divideStackWindow.GetAmount();
                            slot = null; // So no item is equipped
                            //return InsertResult.PartlyStacked;
                            //}
                        }

                        _draggingItemSlot = null;
                    }
                }

                if (cancelledDivide)
                {
                    TweenDraggingIconBack();
                }
                else
                {
                    if (_draggingItemSlot != null)
                    {
                        Log.Debug("Unequipping dragging item...");
                        _draggingItemSlot.Unequip();
                    }
                    if (slot != null)
                    {
                        slot.Equip(DraggingItem);
                        //Log.Debug("Equip result: " + res);
                    }
                    //if (res == IItemSlot.EquipResult.Moved || (res == IItemSlot.EquipResult.Stacked && _draggingItemInfo.Amount == 0) || (res == IItemSlot.EquipResult.PartlyMoved && _draggingItemInfo.Amount == 0))
                    //{
                    //}
                }
                _draggingIcon.QueueFree();
                _draggingIcon = null;
            }
            else
            {
                TweenDraggingIconBack();
            }
            //_slotsUnderMouse[0].Equip(_draggingItem);
        }

        _draggingItemSlot = null;
        DraggingItem = null;

        Log.Debug("Ended release");

        Enable();
    }

    private void TweenDraggingIconBack()
    {
        Tween tween = CreateTween();
        tween.TweenProperty(_draggingIcon, "position", ((Control)_draggingItemSlot).GlobalPosition, 0.1f);
        tween.TweenCallback(Callable.From(_draggingIcon.QueueFree));
        tween.TweenCallback(Callable.From(() => _draggingIcon = null));
    }

    public void AddSlotUnderMouse(IItemSlot itemSlot)
    {
        //GD.Print("add slot under mouse " + ((Control)itemSlot).Name);
        _slotsUnderMouse.Add(itemSlot);

        /*if (!IsProcessing())
        {
            SetProcess(true);
        }*/
    }

    public void RemoveSlotUnderMouse(IItemSlot itemSlot)
    {
        //GD.Print("remove slot under mouse " + ((Control)itemSlot).Name);

        _slotsUnderMouse.Remove(itemSlot);

        /*if (_slotsUnderMouse.Count == 0)
        {
            SetProcess(false);
        }*/
    }

    private void Disable()
    {
        SetProcess(false);
        SetProcessInput(false);
    }

    private void Enable()
    {
        SetProcessInput(true);
    }
}
