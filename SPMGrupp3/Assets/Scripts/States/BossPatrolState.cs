using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boss/BossPatrolState")]
public class BossPatrolState : BondeRangedBaseState
{

    private GameObject target;

    int point = 0;

    // Start is called before the first frame update
    public override void Enter()
    {
        if (owner.patrolPoints.Length > 0)
        {
            target = owner.patrolPoints[point];
            owner.agnes.destination = target.transform.position;
            owner.GetComponent<MeshRenderer>().material.color = Color.white;

            //Debug.Log("DESTINATION: " + owner.agnes.destination);
            //Debug.Log(owner.agnes.updateRotation);
        }


    }



    // Update is called once per frame
    public override void Update()
    {


        if (Vector3.Distance(owner.transform.position, target.transform.position) <= 5.0f)
        {
            //Debug.Log("PATROLMOVE");
            point = (point + 1) % owner.patrolPoints.Length;

            target = owner.patrolPoints[point];
            owner.agnes.destination = target.transform.position;

        }

        if (Vector3.Distance(owner.transform.position, owner.player.transform.position) < owner.maxVisibility)
        {
            //Debug.Log("TRANSITION");
            owner.Transition<BossChaseState>();
        }

    }

}
