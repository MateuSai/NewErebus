namespace Erebus.Weapons;

using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Weapons : Node2D
{
	protected Weapon CurrentWeapon = null;
	protected List<Weapon> WeaponsArray = new();

	/// <summary>
	/// Execute this function to initialize the class
	/// </summary>
	public virtual void Start()
	{

	}

	public void PickUpWeapon(Weapon weapon)
	{
		_ = WeaponsArray.Append(weapon);
		AddChild(weapon);
	}

	public void MoveCurrentWeapon(Vector2 dir)
	{
		CurrentWeapon.Move(dir);
	}

	public virtual void SetCurrentWeapon(Weapon newWeapon)
	{
		CurrentWeapon = newWeapon;
	}
}
