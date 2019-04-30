using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Bonde/BondeStunState")]
public class BondeStunState : BondeBaseState
{

    private float time = 0.0f;

    public override void Enter()
    {
        base.Enter();
        time = 0.0f;
        owner.agnes.isStopped = true;
        owner.agnes.enabled = false;
        owner.GetComponent<Collider>().enabled = false;
        owner.GetComponent<MeshRenderer>().material.color = Color.black;
        
    }

    public override void Update()
    {
        base.Update();
        time += Time.deltaTime;
        if(time % 60 >= owner.stunTime)
        {
            owner.Transition<BondePatrolState>();
        }
    }

    public override void Leave()
    {
        base.Leave();
        owner.agnes.enabled = true;
        owner.GetComponent<Collider>().enabled = true;
        owner.agnes.isStopped = false;

    }
}
