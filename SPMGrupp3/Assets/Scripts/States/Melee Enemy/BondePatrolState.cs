using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Bonde/BondePatrolState")]
public class BondePatrolState : BondeBaseState
{

    [SerializeField] private float patrolPointRange = 1f;
    private GameObject target;
    private bool hasTarget = false;

    int point = 0;

    // Start is called before the first frame update
    public override void Enter()
    {
        base.Enter();
        if (owner.patrolPoints.Length > 0 && owner.patrolPoints[point] != null)
        {
            SetPatrolPoint();
            owner.GetComponent<MeshRenderer>().material.color = Color.white;
            hasTarget = true;

            //Debug.Log("DESTINATION: " + owner.agnes.destination);
            //Debug.Log(owner.agnes.updateRotation);
        }
        


    }

    void SetPatrolPoint()
    {
        target = owner.patrolPoints[point];
        owner.agnes.destination = target.transform.position;
    }

  

    // Update is called once per frame
    public override void Update()
    {
        if(target == null)
        {
            SetPatrolPoint();
        } else if (hasTarget && Vector3.Distance(owner.transform.position, target.transform.position) <= 2.0f)
        {
            
            point = (point + 1) % owner.patrolPoints.Length;

            SetPatrolPoint();
        }

        
        if (Vector3.Distance(owner.transform.position, owner.player.transform.position) < owner.maxVisibility)
        {
            //Debug.Log("TRANSITION");
            owner.Transition<BondeChaseState>();
        }
        
    }

}
