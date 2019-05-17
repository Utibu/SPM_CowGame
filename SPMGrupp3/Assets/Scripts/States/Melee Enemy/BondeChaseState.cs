using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Bonde/BondeChaseState")]
public class BondeChaseState : BondeBaseState
{


    public override void Enter()
    {
        base.Enter();
        owner.agnes.speed = speed * 1.1f;
        owner.agnes.SetDestination(owner.player.transform.position);
       
    }


    // Update is called once per frame
    public override void Update()
    {
        base.Update();
       
        if(owner.agnes != null && owner.agnes.isActiveAndEnabled) {
            owner.agnes.SetDestination(owner.player.transform.position);
        }
        
        // can only go to attack if raycast hits player, thus it can not hit through walls.
        RaycastHit rayHit;
        bool hit = Physics.Raycast(owner.transform.position, (owner.player.transform.position - owner.transform.position).normalized, out rayHit, owner.toAttack);
        if (hit && rayHit.collider.transform.tag.Equals("Player"))
        {
            owner.Transition<BondeAttackState>();
        }
        
        else if(Vector3.Distance(owner.transform.position, owner.player.transform.position) > owner.maxVisibility * 1.1f) // *1.2f för att bonden inte ska flimra mellan states när den står på gränsen.
        {
            owner.Transition<BondePatrolState>();
        }
        

    }
}
