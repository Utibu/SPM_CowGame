using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastingTriggable : Triggable
{

    private Transform currentPlayer;
    private Collider triggerCollider;
    private float distanceToLeave;

    private void Start()
    {
        triggerCollider = GetComponent<Collider>();
        if(triggerCollider.bounds.size.x < triggerCollider.bounds.size.z)
        {
            distanceToLeave = triggerCollider.bounds.size.x / 2;
        } else
        {
            distanceToLeave = triggerCollider.bounds.size.z / 2;
        }
    }

    public override void CustomUpdate()
    {
        base.CustomUpdate();
        
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
