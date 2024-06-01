using Erebus.Characters.Player;
using Erebus.Weapons.Melee.Fists;
using Godot;

namespace Erebus.Weapons;

public partial class PlayerWeapons : Weapons
{
    private Player _player;

    public override void Start()
    {
        base.Start();

        Fists fists = GD.Load<PackedScene>("res://weapons/melee/fists/Fists.tscn").Instantiate<Fists>();
        PickUpWeapon(fists);
        SetCurrentWeapon(fists);
    }

    public override void _Ready()
    {
        base._Ready();

        _player = GetNode<Player>("..");
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);

        if (!CurrentWeapon.IsBusy())
        {
            if (@event.IsActionPressed("ui_attack"))
            {
                CurrentWeapon.Attack();
                return;
            }
        }
    }

    public override void SetCurrentWeapon(Weapon newWeapon)
    {
        base.SetCurrentWeapon(newWeapon);
    }
}