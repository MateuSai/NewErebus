using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ErebusInventory;

public partial class InventorySystem : CanvasLayer
{
    private IItemSlot _draggingItemSlot = null;
    private ItemInfo _draggingItem = null;
    private TextureRect _draggingIcon = null;

    private readonly List<IItemSlot> _slotsUnderMouse = new();

    public InventorySystem()
    {
        Layer = 2; // So the draggins texture appears on top of canvas layers with layer 1
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
            Release();
            SetProcess(false);
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
            return false;
        }

        _draggingItemSlot = _slotsUnderMouse[0];
        _draggingItem = _draggingItemSlot.GetItemInfo();
        _draggingIcon = _draggingItemSlot.GetIconTextureRect();
        _draggingIcon.MouseFilter = Control.MouseFilterEnum.Ignore;
        //_draggingIcon.ZIndex += 50;
        AddChild(_draggingIcon);

        return true;
    }

    private void Release()
    {
        GD.Print("Slots under mouse: " + _slotsUnderMouse.Count);
        if (_slotsUnderMouse.Count == 0)
        {
            Tween tween = CreateTween();
            tween.TweenProperty(_draggingIcon, "position", ((Control)_draggingItemSlot).GlobalPosition, 0.5f);
            tween.TweenCallback(Callable.From(_draggingIcon.QueueFree));
            tween.TweenCallback(Callable.From(() => _draggingIcon = null));
        }
        else
        {
            _draggingIcon.QueueFree();
            _draggingIcon = null;
            _draggingItemSlot.Unequip();
            _slotsUnderMouse[0].Equip(_draggingItem);
        }

        _draggingItemSlot = null;
        _draggingItem = null;
    }

    public void AddSlotUnderMouse(IItemSlot itemSlot)
    {
        GD.Print("add slot under mouse " + ((Control)itemSlot).Name);
        _slotsUnderMouse.Add(itemSlot);

        /*if (!IsProcessing())
        {
            SetProcess(true);
        }*/
    }

    public void RemoveSlotUnderMouse(IItemSlot itemSlot)
    {
        GD.Print("remove slot under mouse " + ((Control)itemSlot).Name);

        _slotsUnderMouse.Remove(itemSlot);

        /*if (_slotsUnderMouse.Count == 0)
        {
            SetProcess(false);
        }*/
    }
}
