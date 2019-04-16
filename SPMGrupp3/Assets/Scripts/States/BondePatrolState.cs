using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Bonde/BondePatrolState")]
public class BondePatrolState : BondeBaseState
{

    private GameObject target;

    int point = 0;

    // Start is called before the first frame update
    public override void Enter()
    {
        if(owner.patrolPoints.Length > 0)
        {
            target = owner.patrolPoints[point];
            owner.agnes.destination = target.transform.position;
        }
        
        
    }

  

    // Update is called once per frame
    public override void Update()
    {
        
        if(Vector3.Distance(owner.transform.position, target.transform.position) <= owner.toAttack)
        {
            point = (point + 1) % owner.patrolPoints.Length;
            
            target = owner.patrolPoints[point];
            owner.agnes.destination = target.transform.position;
            
        }

        if (Vector3.Distance(owner.transform.position, owner.player.transform.position) < owner.maxVisibility - owner.toAttack)
            owner.Transition<BondeChaseState>();
    }

}
