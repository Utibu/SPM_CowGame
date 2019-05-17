﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boss/BossSnipeState")]
public class BossSnipeState : BossBaseState
{
    private float countdown;
    private Vector3 originalPosition;
    [SerializeField] private float attackSpeed;
    private float originalAttackSpeed;
    private float rangeFromDestination = 3f;

    public override void Enter()
    {
        base.Enter();
        originalPosition = owner.transform.position;
        //owner.bulletsShotSinceReload = 0;
        //owner.agnes.Warp(owner.snipeLocation.transform.position);
        //owner.agnes.isStopped = true;
        owner.agnes.enabled = false;
        originalAttackSpeed = owner.attackSpeed;
        owner.attackSpeed = attackSpeed;


    }

    public override void Update()
    {
        lookAt();

        //if (Mathf.Sqrt(owner.transform.position.sqrMagnitude - owner.snipeLocation.transform.position.sqrMagnitude) >= rangeFromDestination)
        if(!Helper.IsWithinDistance(owner.transform.position, owner.snipeLocation.transform.position, rangeFromDestination))
        {
            owner.transform.position = Vector3.Slerp(owner.transform.position, owner.snipeLocation.transform.position, 1.5f * Time.deltaTime);

        }
        else
        {
            owner.agnes.enabled = true;

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
    }

    public override void Leave()
    {
        base.Leave();
        //owner.agnes.isStopped = false;
        //owner.agnes.enabled = true;
        owner.agnes.Warp(originalPosition);
        owner.bulletsShotSinceReload = 0;
        
    }
}
