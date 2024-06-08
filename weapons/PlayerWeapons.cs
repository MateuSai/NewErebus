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

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (!CurrentWeapon.IsBusy())
        {
            if (_player.Velocity.Length() > 2 && CurrentWeapon.GetState() != Weapon.State.Move)
            {
                CurrentWeapon.StartMovingAnimation();
            }
            else if (_player.Velocity.Length() <= 2 && CurrentWeapon.GetState() != Weapon.State.Idle)
            {
                CurrentWeapon.StartIdleAnimation();
            }
        }
    }

    public override void _UnhandledInput(InputEvent @event)
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