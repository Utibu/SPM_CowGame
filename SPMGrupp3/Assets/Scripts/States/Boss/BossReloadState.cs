﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boss/BossReloadState")]
public class BossReloadState : BossBaseState
{

    private float time;

    public override void Enter()
    {
        time = 0f;

    }

    public override void Update()
    {
        if (time % 60 > owner.reloadTime)
        {
            owner.Transition<BossPatrolState>();
        }
        else
        {
            time += Time.deltaTime;
        }

    }

    public override void Leave()
    {
        base.Leave();
    }

}
