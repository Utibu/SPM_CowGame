//Main Author: Niklas Almqvist
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppableObject : Triggable
{
    [HideInInspector] public PlayerValues player;
    protected bool ShouldDestroyOnEnter;

    public void OnCreate(PlayerValues player)
    {
        this.player = GameManager.instance.player.playerValues;
    }

    public override void Start()
    {
        base.Start();
        player = GameManager.instance.player.playerValues;
        ShouldDestroyOnEnter = true;
    }

    public override void OnPlayerTriggerEnter(Collider hitCollider)
    {
        base.OnPlayerTriggerEnter(hitCollider);
        if(ShouldDestroyOnEnter)
        {
            //Destroy(this.gameObject);
            this.gameObject.SetActive(false);
        }
    }
}
