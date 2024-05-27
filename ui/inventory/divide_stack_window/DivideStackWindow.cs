using Erebus.Autoloads;
using ErebusInventory;
using Godot;
using System;
using System.Collections;

namespace Erebus.UI.Inventory.DivideStackWindow;

public partial class DivideStackWindow : PanelContainer
{
    private Globals _globals;
    private HideButton _hideButton;
    private HSlider _amountSlider;
    private TextEdit _amountTextEdit;
    private Button _acceptButton;
    private Button _cancelButton;

    public override void _Ready()
    {
        base._Ready();

        _globals = GetTree().Root.GetNode<Globals>("Globals");
        _hideButton = GetNode<HideButton>("%HideButton");
        _amountSlider = GetNode<HSlider>("%AmountSlider");
        _amountTextEdit = GetNode<TextEdit>("%AmountTextEdit");
        _acceptButton = GetNode<Button>("%AcceptButton");
        _cancelButton = GetNode<Button>("%CancelButton");

        _globals.UI.MoveDarkBackground(1);

        Hidden += () =>
        {
            _amountSlider.Value = 0;
            QueueFree();
        };
        _cancelButton.Pressed += () =>
        {
            _amountSlider.Value = 0;
            QueueFree();
        };

        _acceptButton.Pressed += QueueFree;

        _amountSlider.ValueChanged += (double newValue) =>
        {
            _amountTextEdit.Text = newValue.ToString();
        };
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);

        if (@event.IsActionPressed("ui_accept"))
        {
            QueueFree();
        }
    }

    public override void _ExitTree()
    {
        base._ExitTree();

        _globals.UI.MoveDarkBackground(0);
    }

    public void Setup(ItemInfo item)
    {
        _amountSlider.MaxValue = item.Amount;
    }

    public int GetAmount()
    {
        return (int)_amountSlider.Value;
    }
}
