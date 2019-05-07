using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boss/BossStunState")]
public class BossStunState : BossBaseState
{
    private float time = 0.0f;

    public override void Enter()
    {
        base.Enter();
        time = 0.0f;
        owner.agnes.isStopped = true;
        owner.agnes.enabled = false;
        owner.GetComponent<Collider>().enabled = false;
    }

    public override void Update()
    {
        base.Update();
        owner.GetComponent<MeshRenderer>().material.color = Color.black;
        time += Time.deltaTime;
        if (time % 60 >= owner.stunTime)
        {
            owner.Transition<BossPatrolState>();
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
