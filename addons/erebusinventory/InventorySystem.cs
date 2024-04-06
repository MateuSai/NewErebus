using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ErebusInventory;

public partial class InventorySystem : CanvasLayer
{
    private ItemSlot _draggingItemSlot = null;
    private ItemInfo _draggingItem = null;
    private TextureRect _draggingIcon = null;

    private List<ItemSlot> _slotsUnderMouse = new();

    public override void _Ready()
    {
        base._Ready();

        SetProcess(false);
    }

    public override void _Input(InputEvent @event)
    {
        base._UnhandledInput(@event);

        if (@event.IsActionPressed("grab"))
        {
            GD.Print("Grab");
            Grab();
            SetProcess(true);
        }
        else if (@event.IsActionReleased("grab"))
        {
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

    private void Grab()
    {
        _draggingItemSlot = _slotsUnderMouse[0];
        _draggingItem = _draggingItemSlot.GetItemInfo();
        _draggingIcon = _draggingItemSlot.GetIconTextureRect();
        _draggingIcon.ZIndex += 1;
        AddChild(_draggingIcon);
    }

    private void Release()
    {
        if (_slotsUnderMouse.Count == 0)
        {
            Tween tween = CreateTween();
            tween.TweenProperty(_draggingIcon, "position", _draggingItemSlot.GlobalPosition, 0.5f);
            tween.TweenCallback(Callable.From(_draggingIcon.QueueFree));
            tween.TweenCallback(Callable.From(() => _draggingIcon = null));
        }
        else
        {
            _draggingIcon.QueueFree();
            _draggingIcon = null;
        }

        _draggingItemSlot = null;
        _draggingItem = null;
    }

    public void AddSlotUnderMouse(ItemSlot itemSlot)
    {
        GD.Print("add slot under mouse");
        _slotsUnderMouse.Add(itemSlot);

        /*if (!IsProcessing())
        {
            SetProcess(true);
        }*/
    }

    public void RemoveSlotUnderMouse(ItemSlot itemSlot)
    {
        GD.Print("remove slot under mouse");

        _slotsUnderMouse.Remove(itemSlot);

        /*if (_slotsUnderMouse.Count == 0)
        {
            SetProcess(false);
        }*/
    }
}
