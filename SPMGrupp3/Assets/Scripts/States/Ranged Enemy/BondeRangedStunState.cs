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
        time = 0.0f;
        owner.isDying = false;
        owner.agnes.isStopped = true;
        owner.agnes.enabled = false;
        owner.GetComponent<Collider>().enabled = false;
        owner.GetComponent<MeshRenderer>().material.color = Color.black;
        //owner.transform.Rotate(0, 0, 90);
        owner.transform.position -= new Vector3(0f, owner.StunLengthToGround, 0f); // independant of size, enemy should lie on ground, not float above it.
    }

    public override void Update()
    {
        base.Update();
        owner.transform.eulerAngles = new Vector3(0f, 0f, 90f);
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
        owner.transform.eulerAngles = new Vector3(0f, 0f, 0f);
        owner.transform.position -= new Vector3(0f, -owner.StunLengthToGround, 0f);
        owner.agnes.enabled = true;
        owner.GetComponent<Collider>().enabled = true;
        owner.agnes.isStopped = false;
        owner.DoingKnockback = false;
    }
}
