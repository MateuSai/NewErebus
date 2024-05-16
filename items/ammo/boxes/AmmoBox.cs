using ErebusInventory;
using Godot;
using System;

namespace Erebus.Items.Ammo.Boxes;

public partial class AmmoBox : ItemInfo
{
    public AmmoBox(Texture2D icon, int baseWidth, int baseHeight, short capacity) : base(icon, baseWidth, baseHeight, capacity)
    {
    }
}
