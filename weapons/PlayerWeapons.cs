using Erebus.Weapons.Melee.Fists;
using Godot;

namespace Erebus.Weapons;

public partial class PlayerWeapons : Weapons
{
    public override void _Ready()
    {
        base._Ready();

        Fists fists = GD.Load<PackedScene>("res://weapons/melee/fists/Fists.tscn").Instantiate<Fists>();
        PickUpWeapon(fists);
        CurrentWeapon = fists;
    }
}