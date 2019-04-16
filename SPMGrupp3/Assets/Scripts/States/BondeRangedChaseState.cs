using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BondeRanged/BondeRangedChaseState")]
public class BondeRangedChaseState : BondeRangedBaseState
{
    public float toAttack = 4.0f;
    public float visualRangeMax = 25.0f;

    public override void Enter()
    {
        owner.agnes.speed = speed;
    }


    // Update is called once per frame
    public override void Update()
    {
        owner.GetComponent<MeshRenderer>().material.color = Color.blue;
        owner.agnes.SetDestination(owner.player.transform.position);
        if (Vector3.Distance(owner.transform.position, owner.player.transform.position) > owner.maxVisibility + 1)
        {
            owner.GetComponent<MeshRenderer>().material.color = Color.white;
            owner.Transition<BondeRangedPatrolState>();
        }
        else if (Vector3.Distance(owner.transform.position, owner.player.transform.position) < owner.toAttack)
        {
            owner.GetComponent<MeshRenderer>().material.color = Color.red;
            owner.Transition<BondeRangedAttackState>();

        }
    }
}
