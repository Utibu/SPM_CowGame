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
    private float countdown;
    [SerializeField]private float cooldown = 2.0f;

    public override void Enter()
    {
        base.Enter();
        owner.agnes.speed = 0.1f;
        originalPosition = owner.weapon.transform.rotation;
        countdown = cooldown / 2; // så att denne inte attackerar på en gång
        rotation = 0;
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
            if (owner.itemDrop != null && owner.itemDrop.name.Equals("Key")) // miniboss is the only that drops the key. 
            {
                BossAttack();
            }
            else
            {
                attack();

            }
        }

        if (0.1 <= countdown && countdown <= cooldown / 2)
        {
            rotation += -2;
            owner.weapon.transform.localRotation = Quaternion.Euler(90 + rotation, 0, 0);
        }
        
        RaycastHit rayHit;
        bool hit = Physics.Raycast(owner.transform.position, (owner.player.transform.position - owner.transform.position).normalized, out rayHit, owner.toAttack);
        if (Vector3.Distance(owner.transform.position, owner.player.transform.position) > owner.toAttack || (hit && !rayHit.collider.transform.tag.Equals("Player")))
        {
            owner.Transition<BondeChaseState>();
        }
    }


    private void attack()
    {
        owner.weapon.transform.localRotation = Quaternion.Euler(90,0,0);

        
        if(Vector3.Distance(owner.transform.position, owner.player.transform.position) < owner.toAttack)
        {
            owner.player.playerValues.health -= owner.attackDamage;
            owner.player.velocity += owner.transform.forward * 20f;
        }
        
        // reset cd and move up weapon
        countdown = cooldown;
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
        }
        // reset cd and move up weapon
        countdown = cooldown;
        rotation = 0;
    }
    
    
   
}
