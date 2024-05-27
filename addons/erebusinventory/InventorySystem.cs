using ErebusLogger;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

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

    private readonly List<IItemSlot> _slotsUnderMouse = new();

    public InventorySystem()
    {
        Layer = 2; // So the dragging texture appears on top of canvas layers with layer 1
    }

    public override void _Ready()
    {
        base._Ready();

        SetProcess(false);
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);

        if (@event.IsActionPressed("grab") && _draggingIcon == null)
        {
            GD.Print("Grab");
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
            if (_slotsUnderMouse[0].CanEquip(DraggingItem))
            {
                IItemSlot.EquipResult res = await _slotsUnderMouse[0].Equip(DraggingItem);
                Log.Debug("Equip result: " + res);
                if (res == IItemSlot.EquipResult.Moved || (res == IItemSlot.EquipResult.Stacked && _draggingItemInfo.Amount == 0))
                {
                    _draggingItemSlot.Unequip();
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
