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

    public override void SetCurrentWeapon(Weapon newWeapon)
    {
        base.SetCurrentWeapon(newWeapon);

        string pathToWeapon = "../".PathJoin(_player.GetPathTo(this)).PathJoin(CurrentWeapon.Name);
        //GD.Print(pathToWeapon.PathJoin(CurrentWeapon.GetPathTo(CurrentWeapon.RightHand)));
        _player.SetIKTargets(pathToWeapon.PathJoin(CurrentWeapon.GetPathTo(CurrentWeapon.RightHand)), pathToWeapon.PathJoin(CurrentWeapon.GetPathTo(CurrentWeapon.LeftHand)));
        GD.Print(newWeapon.LeftHandRemoteTransform.GetNode("../../../../../LeftHand").Name);
        newWeapon.LeftHandRemoteTransform.RemotePath = "../../../../../LeftHand";
        newWeapon.RightHandRemoteTransform.RemotePath = "../../../../../RightHand";
        //_player.SetHandRemotes("", "../../Weapons".PathJoin(newWeapon.Name).PathJoin(newWeapon.GetPathTo(newWeapon.LeftHand)));
    }
}