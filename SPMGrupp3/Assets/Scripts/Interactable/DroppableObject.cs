using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppableObject : MonoBehaviour
{
    public PlayerValues player;

    public void OnCreate(PlayerValues player)
    {
        this.player = player;
    }

    private void Start()
    {
        player = GameManager.instance.player.playerValues;
    }

    public virtual void OnEnter()
    {
        Destroy(this.gameObject);
    }
}
