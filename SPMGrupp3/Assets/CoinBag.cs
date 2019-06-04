//Main Author: Joakim Ljung
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBag : CoinPickup
{



    public override void Start()
    {
        base.Start();
        coinAmount = 10;
    }

    
}
