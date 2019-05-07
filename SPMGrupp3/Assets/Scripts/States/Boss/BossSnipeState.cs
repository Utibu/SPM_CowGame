using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boss/BossSnipeState")]
public class BossSnipeState : BossBaseState
{
    private float countdown;

    public override void Enter()
    {
        Debug.Log("in snipe");
        base.Enter();
        owner.bulletsShotSinceReload = 0;
        owner.agnes.isStopped = true;


    }

    public override void Update()
    {
        base.Update();
        owner.transform.position = owner.snipeLocation.transform.position;
        lookAt();

        countdown -= Time.deltaTime;
        //Vector3.Distance(owner.transform.position, owner.player.transform.position) < 4 && 
        if (countdown <= 0)
        {
            attack();
            countdown = owner.attackSpeed + 0.5f;

        }

        if (owner.bulletsShotSinceReload >= owner.bulletsBeforeReload)
        {
            owner.bulletsShotSinceReload = 0;
            owner.Transition<BossAttackState>();
        }

    }

    public override void Leave()
    {
        Debug.Log("leaving snipe");
        base.Leave();
        owner.agnes.isStopped = false;
    }
}
