using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrenthPowerup : DroppableObject
{

    public override void OnPlayerTriggerEnter(Collider hitCollider)
    {
        base.OnPlayerTriggerEnter(hitCollider);
        Debug.Log("LEVEL");
        player.GetComponent<PlayerStateMachine>().dashAirResistance = 0.2f;

    }
}
