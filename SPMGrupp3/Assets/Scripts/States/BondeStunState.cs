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
        owner.agnes.isStopped = true;
    }

    public override void Update()
    {
        base.Update();
        owner.GetComponent<MeshRenderer>().material.color = Color.black;
        time += Time.deltaTime;
        if(time % 60 >= owner.stunTime)
        {
            owner.agnes.isStopped = false;
            owner.Transition<BondePatrolState>();
        }
    }
}
