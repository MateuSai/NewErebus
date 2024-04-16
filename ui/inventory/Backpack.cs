using ErebusInventory;
using Godot;
using System;

namespace Erebus.UI.Inventory;

public partial class Backpack : BodyEquipment
{
    public Backpack(string id, Texture2D icon, int baseWidth, int baseHeight, Texture2D texture) : base(id, icon, baseWidth, baseHeight, texture)
    {
    }
}
