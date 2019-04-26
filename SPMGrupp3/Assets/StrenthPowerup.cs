﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrenthPowerup : DroppableObject
{

    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("LEVEL");
        player.GetComponent<PlayerStateMachine>().dashAirResistance = 0.2f;

    }
}
