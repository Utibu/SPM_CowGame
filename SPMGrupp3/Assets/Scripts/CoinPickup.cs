using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : DroppableObject
{
    //public GameObject player;


    public override void OnEnter()
    {
        base.OnEnter();
        GameManager.instance.coinCount++;        

    }
}
