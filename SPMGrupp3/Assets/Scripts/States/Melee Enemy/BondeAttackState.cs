using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Bonde/BondeAttackState")]
public class BondeAttackState : BondeBaseState
{

    
    
    private float rotation;
    private Quaternion originalPosition;
    Vector3 lookPos;
    public float damage;

    public override void Enter()
    {
        base.Enter();
        Debug.Log("ENTER ATTACK STATE");
        originalPosition = owner.weapon.transform.rotation;
        rotation = 0;
        //owner.agnes.Stop();
        //owner.agnes.speed = 0f;
        if (!owner.customAttackDamage)
        {
            owner.attackDamage = damage;
        }
    }
    public override void Leave()
    {
        //owner.agnes.Resume();
    }

    public override void Update()
    {
        //base.Update();
        //owner.weapon.transform.Rotate(-1, 0, 0);


        owner.agnes.updateRotation = false;
        lookPos = owner.player.transform.position - owner.weapon.transform.position;
        if (Vector3.Distance(new Vector3(owner.player.transform.position.x, 0f, owner.player.transform.position.z), new Vector3(owner.transform.position.x, 0f, owner.transform.position.z)) > 1f)
        {
            lookPos.y = 0;
            Quaternion rot = Quaternion.LookRotation(lookPos);
            owner.transform.rotation = rot;
        }

        owner.countdown -= Time.deltaTime;

        if(Vector3.Distance(owner.transform.position, owner.player.transform.position) < owner.toAttack && owner.countdown <= 0)
        {
            //owner.weapon.transform.localRotation = originalPosition;
            if (owner.itemDrop != null && owner.itemDrop.name.Equals("Key")) // miniboss is the only that drops the key. 
            {
                BossAttack();
                Debug.Log("ATTACK1");
            }
            else
            {
                attack();
                Debug.Log("ATTACK2");
            }
        }

        if (0.1 <= owner.countdown && owner.countdown <= owner.cooldown / 2)
        {
            rotation += -2;
            owner.weapon.transform.localRotation = Quaternion.Euler(90 + rotation, 0, 0);
        }

        //        Debug.Log(Vector3.Distance(owner.transform.position, owner.player.transform.position));
        bool seeEnemy = Physics.Raycast(owner.transform.position, (owner.player.transform.position - owner.transform.position), owner.toAttack, owner.layermask);
        if (Vector3.Distance(owner.transform.position, owner.player.transform.position) > owner.toAttack || !seeEnemy)
        {
            owner.Transition<BondeChaseState>();
        }
    }


    private void attack()
    {
        // move weapon, see if hit
        //owner.weapon.transform.Rotate(110, 0, 0);
        owner.weapon.transform.localRotation = Quaternion.Euler(90,0,0);

        // if hit, do dmg
        //if (owner.weapon.GetComponent<Collider>().bounds.Intersects(owner.player.objectCollider.bounds))
        //{
        if(Vector3.Distance(owner.transform.position, owner.player.transform.position) < owner.toAttack)
        {
            owner.player.playerValues.health -= owner.attackDamage;
            owner.player.velocity += owner.transform.forward * 20f;
            Debug.Log("HIT!");
        }

        //}

        // reset cd and move up weapon
        owner.countdown = owner.cooldown;
        rotation = 0;
    }


    private void BossAttack()
    {
        //owner.weapon.transform.localRotation = Quaternion.Euler(90, 0, 0);
        if (Vector3.Distance(owner.transform.position, owner.player.transform.position) < owner.toAttack)
        {
            owner.player.playerValues.health -= owner.attackDamage;
            owner.player.velocity += owner.transform.forward * 25f;
            owner.player.velocity += new Vector3(0.0f, 15.0f, 0.0f);
            Debug.Log("BOSSHIT!");
        }
        // reset cd and move up weapon
        owner.countdown = owner.cooldown;
        rotation = 0;
    }

    //LEAVE
    
   
}
