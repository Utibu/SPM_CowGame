using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BondeRanged/BondeRangedStunState")]
public class BondeRangedStunState : BondeRangedBaseState
{
    private float time = 0.0f;

    public override void Enter()
    {
        base.Enter();
        owner.transform.Rotate(0, 0, 90);
        owner.transform.position -= new Vector3(0f, 1f, 0f);
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
        //Debug.Log(time);
        if (time >= owner.stunTime)
        {
            //Debug.Log("LEAVING");
            owner.Transition<BondeRangedPatrolState>();
        }
    }

    public override void Leave()
    {
        base.Leave();
        owner.agnes.enabled = true;
        owner.GetComponent<Collider>().enabled = true;
        owner.agnes.isStopped = false;
        owner.transform.Rotate(0, 0, -90);
        owner.transform.position -= new Vector3(0f, -1f, 0f);
    }
}
