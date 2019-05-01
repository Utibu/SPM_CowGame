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

        if (!owner.customAttackDamage)
        {
            owner.attackDamage = damage;
        }

        countdown = owner.attackSpeed;
        attack();
    }

    public override void Update()
    {

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
        if(owner.bulletsShotSinceReload >= owner.bulletsBeforeReload)
        {
            owner.bulletsShotSinceReload = 0;
            owner.Transition<BondeRangedReloadState>();
        }
       // Debug.Log("pew");
    }

    public override void Leave()
    {
        base.Leave();
        owner.agnes.updateRotation = true;
        owner.gun.transform.rotation = owner.transform.rotation;
    }

}
