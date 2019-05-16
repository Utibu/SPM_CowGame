using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boss/BossSnipeState")]
public class BossSnipeState : BossBaseState
{
    private float countdown;
    private Vector3 originalPosition;
    [SerializeField] private float attackSpeed;
    private float originalAttackSpeed;

    public override void Enter()
    {
        Debug.Log("in snipe");
        base.Enter();
        originalPosition = owner.transform.position;
        //owner.bulletsShotSinceReload = 0;
        owner.agnes.isStopped = true;
        owner.agnes.Warp(owner.snipeLocation.transform.position);
        originalAttackSpeed = owner.attackSpeed;
        owner.attackSpeed = attackSpeed;

    }

    public override void Update()
    {
        base.Update();
        lookAt();

        countdown -= Time.deltaTime;
        if (countdown <= 0)
        {
            attack();
            countdown = attackSpeed;

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
