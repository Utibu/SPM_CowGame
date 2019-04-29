using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerStateMachine/DashState")]
public class DashState : PlayerBaseState
{
    private float addToFOV = 20f;
    private float fovChangeVelocity = 10f;
    private float originalFOV;
    private float originalSens;
    public float divideSens = 10f;

    public float dashStateLength = 1f;

    private float timer;


    public override void Enter()
    {
        base.Enter();
        airResistance = ((PlayerStateMachine)owner).dashAirResistance;
        ((PlayerStateMachine)owner).isDashing = true;
        timer = 0.0f;
        //originalFOV = Camera.main.fieldOfView;
        originalSens = ((PlayerStateMachine)owner).mouseSensitivity;
        ((PlayerStateMachine)owner).mouseSensitivity /= divideSens;
    }

    public override void Leave()
    {
        //owner.velocity /= 2f;
        //Camera.main.fieldOfView = originalFOV;
        ((PlayerStateMachine)owner).isDashing = false;
        ((PlayerStateMachine)owner).mouseSensitivity = originalSens;
        ((PlayerStateMachine)owner).lastGravity = gravityConstant;
        ((PlayerStateMachine)owner).lastAcceleration = acceleration;

        base.Leave();
    }

    public override void ActOnCollision(Collider hitCollider)
    {
        base.ActOnCollision(hitCollider);
        if (hitCollider.tag.Equals("Dashable") && owner.velocity.magnitude >= hitCollider.GetComponent<Dashable>().requiredMagnitude)
        {
            if(hitCollider.GetComponent<DroppingObject>() != null)
            {
                hitCollider.GetComponent<DroppingObject>().OnEnter(((PlayerStateMachine)owner).playerValues);
            }
            Destroy(hitCollider.gameObject);
        }
        else if (hitCollider.tag.Equals("Dashable") && owner.velocity.magnitude < hitCollider.GetComponent<Dashable>().requiredMagnitude)

        {
            owner.velocity *= -1;
            owner.Transition<JumpState>();
        }

        if(hitCollider.tag.Equals("DashFallable"))
        {
            hitCollider.GetComponent<FallingObject>().SetFalling(owner.velocity.normalized);
        }

        if(hitCollider.tag.Equals("Enemy"))
        {
            //Destroy(hitCollider.gameObject);
            Bonde bonde = hitCollider.GetComponent<Bonde>();
            bonde.PlayerDash();
        }
        if (hitCollider.tag.Equals("Bounce"))
        {
            owner.velocity *= -1;
            owner.Transition<JumpState>();
        }

    }

    public override void Update()
    {
        base.Update();

        if(timer % 60 > dashStateLength && !((PlayerStateMachine)owner).hasFreeDash)
        {
            owner.Transition<WalkState>();

        } else
        {
            timer += Time.deltaTime;
        }


        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            Jump();
            Debug.Log("JUMPING");
            return;
        }
        else if (!Input.GetKey(KeyCode.LeftShift) || !IsGrounded())
        {
            owner.objectCollider.GetComponent<MeshRenderer>().material.color = Color.white;
            owner.Transition<WalkState>();
            Debug.Log("WALKING NOW");
            return;
        }


        if (owner.velocity.magnitude < ((PlayerStateMachine)owner).velocityToDash)
        {
            owner.Transition<ChargeState>();
            Debug.Log("CARGNING NOW");
        }

        if (Camera.main.fieldOfView <= originalFOV + addToFOV)
        {
           // Camera.main.fieldOfView += fovChangeVelocity * Time.deltaTime;
        }


        

        if(owner.velocity.magnitude > 30)
            owner.objectCollider.GetComponent<MeshRenderer>().material.color = Color.red;
    
        else
            owner.objectCollider.GetComponent<MeshRenderer>().material.color = Color.black;


        

    }
}
