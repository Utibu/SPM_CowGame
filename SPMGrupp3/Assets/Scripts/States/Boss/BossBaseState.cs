﻿//Main Author: Niklas Almqvist
//Secondary Author: Joakim Ljung
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBaseState : EnemyGeneralState
{

    protected BossStateMachine owner;
    public float speed;
    public float bulletAcceleration;

    // for collision
    private Vector3 movement;


    public override void Update()
    {
        if (owner.isDying)
        {
            owner.Transition<BossStunState>();
            owner.isDying = false;
            return;
        }

        if (owner.ShouldGoAlive)
        {
            owner.Transition<BossPatrolState>();
            owner.ShouldGoAlive = false;
            return;
        }
    }

    public override void Initialize(StateMachine stateMachine)
    {
        owner = (BossStateMachine)stateMachine;
    }

    public override void Enter()
    {

    }

    protected void attack()
    {
        GameObject bullet = Instantiate(owner.bullet, owner.ActiveWeapon.transform.position, Quaternion.identity);
        BulletStateMachine stateMachine = bullet.GetComponent<BulletStateMachine>();
        stateMachine.hasKnockback = true;
        stateMachine.SendBullet(owner.ActiveWeapon.transform.forward.normalized * bulletAcceleration, owner.attackDamage);
        owner.bulletsShotSinceReload++;
        EventSystem.Current.FireEvent(new PlaySoundEvent(owner.transform.position, owner.GunSound, 1f, 0.9f, 1.1f));
    }

    protected void lookAt()
    {

        owner.agnes.updateRotation = false;
        Vector3 lookPos = owner.player.transform.position - owner.transform.position;

        if (Vector3.Distance(new Vector3(owner.player.transform.position.x, 0f, owner.player.transform.position.z), new Vector3(owner.transform.position.x, 0f, owner.transform.position.z)) > 1f)
        {
            lookPos.y = 0;
            Quaternion rotation = Quaternion.LookRotation(lookPos);
            owner.transform.rotation = rotation;
        }



        Vector3 gunPos = owner.player.transform.position - owner.ActiveWeapon.transform.position;
        Quaternion gunRotation = Quaternion.LookRotation(gunPos);
        owner.ActiveWeapon.transform.rotation = gunRotation;
    }

    
}
