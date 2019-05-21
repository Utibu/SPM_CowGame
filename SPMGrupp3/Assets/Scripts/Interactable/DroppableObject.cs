using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppableObject : Triggable
{
    public PlayerValues player;

    public void OnCreate(PlayerValues player)
    {
        this.player = GameManager.instance.player.playerValues;
    }

    private void Start()
    {
        player = GameManager.instance.player.playerValues;
    }

    public override void OnPlayerTriggerEnter(Collider hitCollider)
    {
        base.OnPlayerTriggerEnter(hitCollider);
        //Destroy(this.gameObject);
    }
}
