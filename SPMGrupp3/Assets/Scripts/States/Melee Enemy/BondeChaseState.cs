using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Bonde/BondeChaseState")]
public class BondeChaseState : BondeBaseState
{


    public override void Enter()
    {
        base.Enter();
    }


    // Update is called once per frame
    public override void Update()
    {
        if(owner.agnes != null && owner.agnes.isActiveAndEnabled) {
            owner.agnes.SetDestination(owner.player.transform.position);
        }

        // can only go to attack if raycast hits player, thus it can not hit through walls.
        if (Physics.BoxCast(owner.transform.position, owner.GetComponent<BoxCollider>().center, (owner.transform.position - owner.player.transform.position), owner.transform.rotation, owner.toAttack))
        {
            owner.Transition<BondeAttackState>();
        }
        
        else if(Vector3.Distance(owner.transform.position, owner.player.transform.position) > owner.maxVisibility)
        {
            owner.Transition<BondePatrolState>();
        }
        
    }
}
