using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BondeRangedStateMachine : Bonde
{
    public GameObject bullet;
    public GameObject gun;
    public float attackSpeed;

    public override void PlayerDash()
    {
        Transition<BondeRangedStunState>();
    }
}
