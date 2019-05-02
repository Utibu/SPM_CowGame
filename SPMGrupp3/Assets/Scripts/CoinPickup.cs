using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : DroppableObject
{

    public override void OnEnter()
    {
        base.OnEnter();
        GameManager.instance.coinCount++;
    }
}
