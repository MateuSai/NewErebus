using Godot;
using System;

namespace Erebus.UI.LootAndStashWindow;

public partial class LootTab : ScrollContainer
{
    public override void _Ready()
    {
        base._Ready();

        GetVScrollBar().CustomMinimumSize = new(9, 0);
    }
}
