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

    public override void Initialize(StateMachine stateMachine)
    {
        base.Initialize(stateMachine);
        player.dashAirResistance = airResistance;
        player.dashStateAcceleration = acceleration;
        player.dashStateGravity = gravityConstant;
    }

    public override void Enter()
    {
        base.Enter();
        jumpForce = player.dashJumpForce;
        airResistance = player.dashAirResistance;
        player.isDashing = true;
        timer = 0.0f;
        //originalFOV = Camera.main.fieldOfView;
        originalSens = player.mouseSensitivity;
        player.mouseSensitivity /= divideSens;

        player.dashAirResistance = airResistance;
        player.dashStateAcceleration = acceleration;
        player.dashStateGravity = gravityConstant;
    }

    public override void Leave()
    {
        //owner.velocity /= 2f;
        //Camera.main.fieldOfView = originalFOV;
        player.isDashing = false;
        player.ResetDash();
        player.mouseSensitivity = originalSens;
        player.lastGravity = gravityConstant;
        player.lastAcceleration = acceleration;

        base.Leave();
    }

    public override void ActOnCollision(Collider hitCollider)
    {
        base.ActOnCollision(hitCollider);
        if (hitCollider.tag.Equals("Dashable") && owner.velocity.magnitude >= hitCollider.GetComponent<Dashable>().requiredMagnitude)
        {
            if(hitCollider.GetComponent<DroppingObject>() != null)
            {
                hitCollider.GetComponent<DroppingObject>().OnEnter(player.playerValues);
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

        if (hitCollider.tag.Equals("JumpBale"))
        {
            Collider col = GetGroundCollider();
            if (col != null)
            {
                if (col.tag.Equals("JumpBale"))
                {
                    owner.Transition<JumpBaleState>();

                }
                else
                {
                    if (hitCollider.GetComponent<BarrellStateMachine>() != null)
                    {
                        hitCollider.GetComponent<BarrellStateMachine>().Move(owner.velocity);
                    }
                }
            }
        } else if (hitCollider.GetComponent<BarrellStateMachine>() != null)
        {
            hitCollider.GetComponent<BarrellStateMachine>().Move(owner.velocity);
        }

    }

    public override void Update()
    {
        base.Update();

        if(timer > dashStateLength && !player.hasFreeDash)
        {
            owner.Transition<WalkState>();

        } else
        {
            timer += Time.deltaTime;
            if (GameManager.instance.dashCooldownImage != null)
            {
                GameManager.instance.dashCooldownImage.fillAmount -= timer / dashStateLength;
            }
        }


        if (GameManager.instance.inputManager.JumpKeyDown() && IsGrounded())
        {
            Jump();
            Debug.Log("JUMPING");
            return;
        }
        else if (!GameManager.instance.inputManager.DashKey() || !IsGrounded())
        {
            owner.objectCollider.GetComponent<MeshRenderer>().material.color = Color.white;
            owner.Transition<WalkState>();
            Debug.Log("WALKING NOW");
            return;
        }


        if (owner.velocity.magnitude < player.velocityToDash)
        {
            owner.Transition<ChargeState>();
            Debug.Log("CARGNING NOW");
        }

        if (Camera.main.fieldOfView <= originalFOV + addToFOV)
        {
           // Camera.main.fieldOfView += fovChangeVelocity * Time.deltaTime;
        }


        

        if(owner.velocity.magnitude > player.toSuperDash)
            owner.objectCollider.GetComponent<MeshRenderer>().material.color = Color.red;
    
        else
            owner.objectCollider.GetComponent<MeshRenderer>().material.color = Color.black;


        

    }
}
