using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ErebusInventory;

public partial class InventorySystem : CanvasLayer
{
    private IItemSlot _draggingItemSlot = null;
    public ItemInfo DraggingItem = null;
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

    private void Release()
    {
        //GD.Print("Slots under mouse: " + _slotsUnderMouse.Count);
        if (_slotsUnderMouse.Count == 0)
        {
            TweenDraggingIconBack();
        }
        else
        {
            if (_slotsUnderMouse[0].CanEquip(DraggingItem))
            {
                _draggingItemSlot.Unequip();
                _slotsUnderMouse[0].Equip(DraggingItem);
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
}
