using Erebus.Autoloads;
using Godot;
using System;

namespace Erebus.UI.Inventory.DivideStackWindow;

public partial class DivideStackWindow : PanelContainer
{
    private Globals _globals;
    private HideButton _hideButton;
    public TextEdit AmountTextEdit;

    public override void _Ready()
    {
        base._Ready();

        _globals = GetTree().Root.GetNode<Globals>("Globals");
        _hideButton = GetNode<HideButton>("%HideButton");
        AmountTextEdit = GetNode<TextEdit>("%AmountTextEdit");

        _globals.UI.MoveDarkBackground(1);

        Hidden += QueueFree;
    }

    public override void _ExitTree()
    {
        base._ExitTree();

        _globals.UI.MoveDarkBackground(0);
    }
}
