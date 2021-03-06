﻿//Main Author: Niklas Almqvist
//Secondary Author: Sofia Kauko
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BondeRanged/BondeRangedAttackState")]
public class BondeRangedAttackState : BondeRangedBaseState
{

    public float bulletAcceleration = 20f;
    //public float cooldown = 1.2f;
    private float countdown;
    public float damage;

    public override void Enter()
    {
        base.Enter();
        if (!owner.customAttackDamage)
        {
            owner.attackDamage = damage;
        }

        
        owner.agnes.isStopped = true;

        countdown = owner.attackSpeed/2;
    }

    public override void Update()
    {
        base.Update();
        owner.agnes.updateRotation = false;
        Vector3 lookPos = owner.player.transform.position - owner.transform.position;

        if(Vector3.Distance(new Vector3(owner.player.transform.position.x, 0f, owner.player.transform.position.z), new Vector3(owner.transform.position.x, 0f, owner.transform.position.z)) > 1f)
        {
            lookPos.y = 0;
            Quaternion rotation = Quaternion.LookRotation(lookPos);
            owner.transform.rotation = rotation;
        }

        

        Vector3 gunPos = owner.player.transform.position - owner.gun.transform.position;
        Quaternion gunRotation = Quaternion.LookRotation(gunPos);
        owner.gun.transform.rotation = gunRotation;

        countdown -= Time.deltaTime;
        //Vector3.Distance(owner.transform.position, owner.player.transform.position) < 4 && 
        if (countdown <= 0)
        {
            attack();
            countdown = owner.attackSpeed;

        }

        if (Vector3.Distance(owner.transform.position, owner.player.transform.position) > owner.toAttack + 2)
        {
            owner.Transition<BondeRangedChaseState>();
        }
    }


    private void attack()
    {
        GameObject bullet = Instantiate(owner.bullet, owner.gun.transform.position, Quaternion.identity);
        BulletStateMachine stateMachine = bullet.GetComponent<BulletStateMachine>();
        stateMachine.SendBullet(owner.gun.transform.forward.normalized * bulletAcceleration, owner.attackDamage);
        owner.bulletsShotSinceReload++;
        EventSystem.Current.FireEvent(new PlaySoundEvent(owner.transform.position, owner.GunSound, 1f, 0.9f, 1.1f));
        if (owner.bulletsShotSinceReload >= owner.bulletsBeforeReload && owner.DoingKnockback == false)
        {
            owner.bulletsShotSinceReload = 0;
            owner.Transition<BondeRangedReloadState>();
        }
       // Debug.Log("pew");
    }

    public override void Leave()
    {
        base.Leave();
        owner.agnes.isStopped = false;
        owner.agnes.updateRotation = true;
        owner.gun.transform.rotation = owner.transform.rotation;
    }

}
