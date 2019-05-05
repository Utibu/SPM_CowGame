using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boss/BossSnipeState")]
public class BossSnipeState : BossAttackState
{
    private float bulletsToReloadBefore = 1000;

    public override void Enter()
    {
        base.Enter();
        bulletsToReloadBefore = owner.bulletsBeforeReload;
        owner.transform.position = owner.snipeLocation.transform.position;
        Debug.Log("enter snipe");
        
    }

    public override void Update()
    {
        base.Update();
        owner.agnes.isStopped = true;
        
    }

    public override void Leave()
    {
        Debug.Log("leaving snipe");
        base.Leave();
        owner.agnes.isStopped = false;
        owner.bulletsBeforeReload = bulletsToReloadBefore;
    }
}
