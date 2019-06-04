//Main Author: Sofia Kauko
//Secondary Author: Niklas Almqvist
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

        
        owner.isDying = false;
        owner.agnes.isStopped = true;
        owner.agnes.enabled = false;
        owner.GetComponent<Collider>().enabled = false;
        owner.GetComponent<MeshRenderer>().material.color = Color.black;
        //owner.transform.Rotate(0, 0, 90);
        owner.transform.position -= new Vector3(0f, owner.StunLengthToGround, 0f) ; // independant of size, enemy should lie on ground, not float above it.

        if(owner.itemDrop != null)
        {
            Instantiate(owner.itemDrop, owner.transform.position + new Vector3(0f, 2f, 0f), Quaternion.identity);
            //owner.itemDrop.transform.position = owner.transform.position; // + new Vector3(0.5f, 0.0f, 0.5f)
        }
        if(owner.healthMeter != null)
        {
            
        }
        
    }

    public override void Update()
    {
        base.Update();
        owner.transform.eulerAngles = new Vector3(0f, 0f, 90f);
        time += Time.deltaTime;
        if(time >= owner.stunTime)
        {
            owner.Transition<BondePatrolState>();
        }
    }

    public override void Leave()
    {
        base.Leave();
        //owner.transform.Rotate(0, 0, -90);
        owner.transform.eulerAngles = new Vector3(0f, 0f, 0f);
        owner.GetComponent<MeshRenderer>().material.color = Color.white;
        owner.transform.position -= new Vector3(0f, -owner.StunLengthToGround, 0f);
        owner.agnes.enabled = true;
        owner.DoingKnockback = false;
        owner.GetComponent<Collider>().enabled = true;
        owner.agnes.isStopped = false;
    }
}
