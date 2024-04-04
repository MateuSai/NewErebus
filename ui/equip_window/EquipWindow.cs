using Godot;
using System;

namespace Erebus.UI.EquipWindow;

public partial class EquipWindow : Window
{
    public override void _Ready()
    {
        base._Ready();

        CloseRequested += Hide;
    }
}
