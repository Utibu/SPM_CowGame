using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boss/BossSnipeState")]
public class BossSnipeState : BossBaseState
{
    private float countdown;
    private Vector3 originalPosition;

    public override void Enter()
    {
        Debug.Log("in snipe");
        base.Enter();
        originalPosition = owner.transform.position;
        owner.bulletsShotSinceReload = 0;
        owner.agnes.isStopped = true;
        owner.agnes.Warp(owner.snipeLocation.transform.position);



    }

    public override void Update()
    {
        base.Update();
        lookAt();

        countdown -= Time.deltaTime;
        if (countdown <= 0)
        {
            attack();
            countdown = owner.attackSpeed + 0.5f;

        }
        /*
        if (owner.bulletsShotSinceReload >= owner.bulletsBeforeReload)
        {
            owner.bulletsShotSinceReload = 0;
            owner.Transition<BossAttackState>();
        }
        */
    }

    public override void Leave()
    {
        Debug.Log("leaving snipe");
        base.Leave();
        owner.agnes.isStopped = false;
        owner.agnes.Warp(originalPosition);
    }
}
