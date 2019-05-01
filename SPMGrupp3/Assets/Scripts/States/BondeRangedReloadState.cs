using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BondeRanged/BondeRangedReloadState")]
public class BondeRangedReloadState : BondeRangedBaseState
{

    private float time;

    public override void Enter()
    {
        time = 0f;
    }

    public override void Update()
    {
        if(time % 60 > owner.reloadTime)
        {
            owner.Transition<BondeRangedPatrolState>();
        } else
        {
            time += Time.deltaTime;
        }
     
    }

    public override void Leave()
    {
        base.Leave();
    }

}
