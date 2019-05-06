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
        GameObject bullet = Instantiate(owner.bullet, owner.gun.transform.position, Quaternion.identity);
        BulletStateMachine stateMachine = bullet.GetComponent<BulletStateMachine>();
        stateMachine.SendBullet(owner.gun.transform.forward.normalized * bulletAcceleration, owner.attackDamage);
        owner.bulletsShotSinceReload++;
        
        // Debug.Log("pew");
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



        Vector3 gunPos = owner.player.transform.position - owner.gun.transform.position;
        Quaternion gunRotation = Quaternion.LookRotation(gunPos);
        owner.gun.transform.rotation = gunRotation;
    }

    
}
