//Main Author: Niklas Almqvist
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrenthPowerup : DroppableObject
{
    private bool isShowingTrigger = false;
    private UIMessageTrigger messageTrigger = null;

    public override void Start()
    {
        base.Start();
        ShouldDestroyOnEnter = false;
    }

    public override void OnPlayerTriggerEnter(Collider hitCollider)
    {
        base.OnPlayerTriggerEnter(hitCollider);
        GameManager.instance.player.DashLevel++;
        //player.GetComponent<PlayerStateMachine>().dashAirResistance = 0.2f;
        messageTrigger = GetComponent<UIMessageTrigger>();
        if (messageTrigger != null)
        {
            messageTrigger.ShowMessage();
        } else
        {
            Destroy(gameObject);
        }

    }

    public override void Update()
    {
        base.Update();
        if(isShowingTrigger && messageTrigger != null)
        {
            if(messageTrigger.IsShowing == false)
            {
                Destroy(gameObject);
            }
        }
    }
}
