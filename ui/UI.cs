using Godot;
using System;

namespace Erebus.UI;

public partial class UI : CanvasLayer
{
    private EquipWindow.EquipWindow _equipWindow;

    public override void _Ready()
    {
        base._Ready();

        _equipWindow = GetNode<EquipWindow.EquipWindow>("VBoxContainer/EquipWindow");
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        base._UnhandledInput(@event);

        if (@event.IsActionPressed("inventory"))
        {
            _equipWindow.Visible = !_equipWindow.Visible;
        }
    }
}
