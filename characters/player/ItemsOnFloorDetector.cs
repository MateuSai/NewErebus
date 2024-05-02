using Erebus.Items;
using Godot;
using System;
using System.Collections.Generic;

namespace Erebus.Characters.Player;

public partial class ItemsOnFloorDetector : Area2D
{
    private readonly List<ItemOnFloor> _itemsOnFloor = new();

    public List<ItemOnFloor> GetItemsOnFloor()
    {
        return _itemsOnFloor;
    }

    public override void _Ready()
    {
        base._Ready();

        AreaEntered += OnAreaEntered;
        AreaExited += OnAreaExited;
    }

    private void OnAreaEntered(Area2D area)
    {
        ItemOnFloor itemOnFloor = (ItemOnFloor)area;
        _itemsOnFloor.Add(itemOnFloor);
    }

    private void OnAreaExited(Area2D area)
    {
        ItemOnFloor itemOnFloor = (ItemOnFloor)area;
        _itemsOnFloor.Remove(itemOnFloor);
    }
}
