using Godot;
using System;

namespace Erebus.UI;

[Tool, GlobalClass]
public partial class HideButton : TextureButton
{
    public HideButton()
    {
        TextureNormal = GD.Load<Texture2D>("res://art/ui/Tiny_close_button_normal.png");
        TextureHover = GD.Load<Texture2D>("res://art/ui/Tiny_close_button_hover.png");
        TexturePressed = GD.Load<Texture2D>("res://art/ui/Tiny_close_button_pressed.png");
    }

    public override void _Ready()
    {
        base._Ready();

        Pressed += GetOwner<Control>().Hide;
    }
}
