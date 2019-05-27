using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boss/BossAttackState")]
public class BossAttackState : BossBaseState
{

    //public float bulletAcceleration = 20f;
    //public float cooldown = 1.2f;
    private float countdown;
    public float damage;

    public override void Enter()
    {
        if (!owner.customAttackDamage)
        {
            owner.attackDamage = damage;
        }

        countdown = owner.attackSpeed;
        attack();
    }

    public override void Update()
    {

        lookAt();

        countdown -= Time.deltaTime;
        //Vector3.Distance(owner.transform.position, owner.player.transform.position) < 4 && 
        if (countdown <= 0)
        {
            attack();
            countdown = owner.attackSpeed;

        }

        if (owner.bulletsShotSinceReload >= owner.bulletsBeforeReload)
        {
            owner.bulletsShotSinceReload = 0;
            owner.Transition<BossReloadState>();
        }

        if (Vector3.Distance(owner.transform.position, owner.player.transform.position) > owner.toAttack + 2)
        {
            owner.Transition<BossChaseState>();
        }
    }


    

    public override void Leave()
    {
        base.Leave();
        owner.agnes.updateRotation = true;
        owner.ActiveWeapon.transform.rotation = owner.transform.rotation;
    }

}
