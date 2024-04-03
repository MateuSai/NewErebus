namespace Erebus.Weapons;

using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Weapons : Node2D
{
    protected Weapon CurrentWeapon = null;
    protected List<Weapon> WeaponsArray = new();

    public void PickUpWeapon(Weapon weapon)
    {
        _ = WeaponsArray.Append(weapon);
        AddChild(weapon);
    }
}
