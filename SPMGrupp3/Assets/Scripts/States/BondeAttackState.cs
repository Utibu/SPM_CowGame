using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Bonde/BondeAttackState")]
public class BondeAttackState : BondeBaseState
{

    private float cooldown = 1.2f;
    private float countdown;
    private bool attacking;
    private float rotation;
    private Quaternion originalPosition;
    Vector3 lookPos;
    public float damage;

    public override void Enter()
    {
        base.Enter();
        originalPosition = owner.weapon.transform.rotation;
        rotation = 0;
        //owner.agnes.Stop();
        //owner.agnes.speed = 0f;
        if (!owner.customAttackDamage)
        {
            owner.attackDamage = damage;
        }
        countdown = cooldown;
        attacking = false;
        attack();
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

        countdown -= Time.deltaTime;

        if(Vector3.Distance(owner.transform.position, owner.player.transform.position) < owner.toAttack && countdown <= 0)
        {
            //owner.weapon.transform.localRotation = originalPosition;
            if (owner.itemDrop != null && owner.itemDrop.name.Equals("Key")) // miniboss is the only that drops the key. 
            {
                BossAttack();
            }
            attack();
        }

        if (0.1 <= countdown && countdown <= cooldown/2)
        {
            rotation += -2;
            owner.weapon.transform.localRotation = Quaternion.Euler(90 + rotation, 0, 0);
        }

//        Debug.Log(Vector3.Distance(owner.transform.position, owner.player.transform.position));
        if(Vector3.Distance(owner.transform.position, owner.player.transform.position) > owner.toAttack)
        {
            owner.Transition<BondeChaseState>();
        }
    }


    private void attack()
    {
        attacking = true;
        // move weapon, see if hit
        //owner.weapon.transform.Rotate(110, 0, 0);
        owner.weapon.transform.localRotation = Quaternion.Euler(90,0,0);

        // if hit, do dmg
        //if (owner.weapon.GetComponent<Collider>().bounds.Intersects(owner.player.objectCollider.bounds))
        //{
        if(Vector3.Distance(owner.transform.position, owner.player.transform.position) < owner.toAttack / 2)
        {
            owner.player.playerValues.health -= owner.attackDamage;
            owner.player.velocity += owner.transform.forward * 20f;
            Debug.Log("HIT!");
        }
            
        //}

        // reset cd and move up weapon
        countdown = cooldown;
        attacking = false;
        rotation = 0;
    }


    private void BossAttack()
    {
        attacking = true;
        owner.weapon.transform.localRotation = Quaternion.Euler(90, 0, 0);
        if (Vector3.Distance(owner.transform.position, owner.player.transform.position) < owner.toAttack / 2)
        {
            owner.player.playerValues.health -= owner.attackDamage;
            owner.player.velocity += owner.transform.forward * 45f;
            owner.player.velocity += new Vector3(0.0f, 3.0f, 0.0f);
            Debug.Log("HIT!");
        }
        // reset cd and move up weapon
        countdown = cooldown;
        attacking = false;
        rotation = 0;
    }

    //LEAVE
    
   
}
