using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrenthPowerup : DroppableObject
{

    public override void OnPlayerTriggerEnter(Collider hitCollider)
    {
        base.OnPlayerTriggerEnter(hitCollider);
        GameManager.instance.player.DashLevel++;
        //player.GetComponent<PlayerStateMachine>().dashAirResistance = 0.2f;

    }
}
