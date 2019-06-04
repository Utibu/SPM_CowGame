//Main Author: Sofia Kauko
//Secondary Author: Niklas Almqvist
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Bonde/BondePatrolState")]
public class BondePatrolState : BondeBaseState
{

    [SerializeField] private float patrolPointRange = 1f;
    private GameObject target;
    private bool hasTarget = false;
    int patrolPointIndex = 0;
    MeshRenderer meshRenderer = null;

    // Start is called before the first frame update
    public override void Enter()
    {
        base.Enter();
        owner.agnes.speed = speed;
        if (owner.patrolPoints.Length > 0 && owner.patrolPoints[patrolPointIndex] != null)
        {
            SetPatrolPoint();
            meshRenderer = owner.GetComponent<MeshRenderer>();
            meshRenderer.material.color = Color.white;
            hasTarget = true;
        }

    }

    void SetPatrolPoint()
    {
            
        target = owner.patrolPoints[patrolPointIndex];
        if(target != null)
        {
            hasTarget = true;
            owner.agnes.destination = target.transform.position;
        }
           
    }
    

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if(owner.GetCurrentState().GetType() != typeof(BondePatrolState))
        {
            return;
        }

        if (meshRenderer != null && meshRenderer.material.color != Color.white)
        {
            meshRenderer.material.color = Color.white;
        }

        RaycastHit rayHit;
        bool hit = Physics.Raycast(owner.transform.position, (owner.player.transform.position - owner.transform.position).normalized, out rayHit, owner.maxVisibility);
        if (hit && rayHit.collider.tag.Equals("Player"))
        {
            owner.Transition<BondeChaseState>();
        }
        else if (hasTarget && Vector3.Distance(owner.transform.position, target.transform.position) <= 2.0f)
        {
            patrolPointIndex = (patrolPointIndex + 1) % owner.patrolPoints.Length;
            SetPatrolPoint();
        }
        else if (target == null)
        {
            SetPatrolPoint();
        }

        
    }

}
