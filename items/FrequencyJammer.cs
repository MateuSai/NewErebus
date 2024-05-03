using ErebusInventory;
using Godot;
using System;

namespace Erebus.Items;

public partial class FrequencyJammer : ItemInfo
{
    public FrequencyJammer() : base(GD.Load<Texture2D>("res://art/ui/inventory_icons/Frequency_jammer.png"), 2, 2)
    {
    }
}
