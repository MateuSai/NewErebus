using ErebusInventory;
using Godot;
using System;

namespace Erebus.UI.Inventory;

public partial class Helmet : BodyEquipment
{
    public Helmet(string id, Texture2D icon, int baseWidth, int baseHeight, Texture2D texture) : base(id, icon, baseWidth, baseHeight, texture)
    {
    }
}
