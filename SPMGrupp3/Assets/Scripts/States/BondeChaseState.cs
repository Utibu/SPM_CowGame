using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Bonde/BondeChaseState")]
public class BondeChaseState : BondeBaseState
{


    public override void Enter()
    {
        owner.agnes.speed = 10f;
    }


    // Update is called once per frame
    public override void Update()
    {
        owner.agnes.SetDestination(owner.player.transform.position);
        if(Vector3.Distance(owner.transform.position, owner.player.transform.position) > owner.maxVisibility)
        {
            owner.Transition<BondePatrolState>();
        }
        else if(Vector3.Distance(owner.transform.position, owner.player.transform.position) < owner.toAttack){
            owner.Transition<BondeAttackState>();
            
        }
    }
}
