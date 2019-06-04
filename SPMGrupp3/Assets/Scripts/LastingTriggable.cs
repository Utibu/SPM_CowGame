//Main Author: Niklas Almqvist
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastingTriggable : Triggable
{

    private Transform currentPlayer;
    private float distanceToLeave;

    public override void Start()
    {
        base.Start();
        if(TriggerCollider.bounds.size.x < TriggerCollider.bounds.size.z)
        {
            distanceToLeave = TriggerCollider.bounds.size.x / 2;
        } else
        {
            distanceToLeave = TriggerCollider.bounds.size.z / 2;
        }
    }

    public override void Update()
    {
        base.Update();
        
        if(currentPlayer != null)
        {
            if (Helper.IsWithinDistance(currentPlayer.position, transform.position, distanceToLeave) == false)
            {
                OnPlayerTriggerLeave();
            }
        }
    }

    protected virtual void OnPlayerTriggerLeave()
    {
        currentPlayer = null;
        Debug.Log("LEAVING");
    }

    public override void OnPlayerTriggerEnter(Collider hitCollider)
    {
        base.OnPlayerTriggerEnter(hitCollider);
        currentPlayer = GameManager.instance.player.transform;
    }
}
