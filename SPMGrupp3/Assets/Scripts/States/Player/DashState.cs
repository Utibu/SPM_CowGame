﻿using System.Collections;
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

    //private BasicTimer dashTimer = new BasicTimer(2f);
    private Dashable dashable;


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
        airResistance = player.dashAirResistance;
        player.isDashing = true;
        //originalFOV = Camera.main.fieldOfView;
        originalSens = player.mouseSensitivity;
        player.mouseSensitivity /= divideSens;

        player.dashAirResistance = airResistance;
        player.dashStateAcceleration = acceleration;
        player.dashStateGravity = gravityConstant;
        UIManager.instance.SetDashFillAmount(1f);
        player.DashCooldownTimer.Reset();
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

        player.DashCooldownTimer.Reset();
        if(player.DashDurationTimer.GetPercentage() > 0)
        {
            player.DashCooldownTimer.SetStartTime((1 - player.DashDurationTimer.GetPercentage()) * player.DashCooldownTimer.GetDuration());
        } else
        {
            player.DashCooldownTimer.SetStartTime(0);
        }
        
        player.DashDurationTimer.Reset();

        base.Leave();
    }

    public override void ActOnCollision(Collider hitCollider, out bool skipCollision)
    {
        base.ActOnCollision(hitCollider, out skipCollision);


        if(hitCollider.GetComponent<Dashable>() != null)
        {
            hitCollider.GetComponent<Dashable>().OnPlayerCollideEnter(hitCollider, out skipCollision, player.DashLevel);
        }

        /*
        if (hitCollider.tag.Equals("Dashable") && player.DashLevel >= hitCollider.GetComponent<Dashable>().requiredLevel)
        {
            if(hitCollider.GetComponent<DroppingObject>() != null)
            {
                hitCollider.GetComponent<DroppingObject>().OnEnter(player.playerValues);
            }
            dashable = hitCollider.GetComponent<Dashable>();
            if (dashable.GetClip() != null)
            {
                //Debug.Log("object ram");
                EventSystem.Current.FireEvent(new PlaySoundEvent(dashable.gameObject.transform.position, dashable.GetClip(), 1f, 0.8f, 1.1f));
            }
            Destroy(hitCollider.gameObject);
            skipCollision = true;
        }
        else if (hitCollider.tag.Equals("Dashable") && player.DashLevel < hitCollider.GetComponent<Dashable>().requiredLevel)
        {
            owner.velocity *= -1;
        }
        */
        /*
        if(hitCollider.GetComponent<Collidable>() != null)
        {
            hitCollider.GetComponent<Collidable>().OnPlayerCollideEnter(hitCollider, out skipCollision);
        }
        */
        /*
        if(hitCollider.tag.Equals("DashFallable"))
        {
            hitCollider.GetComponent<FallingObject>().SetFalling(owner.velocity.normalized);
        }
        */
        if (hitCollider.tag.Equals("Enemy"))
        {
            //Destroy(hitCollider.gameObject);
            Bonde bonde = hitCollider.GetComponent<Bonde>();
            bonde.PlayerDash();
        }
        /*
        if (hitCollider.tag.Equals("Bounce"))
        {
            owner.velocity *= -1;
            owner.Transition<JumpBaleState>();
        }
        */

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
        }

        if (hitCollider.tag.Equals("Barrell"))
        {
            Collider col = GetGroundCollider();
            if (col != null)
            {
                if (col.tag.Equals("Barrell"))
                {
                    //hitCollider.GetComponent<BarrellStateMachine>().Move(owner.velocity);
                }
                else
                {
                    if (hitCollider.GetComponent<BarrellStateMachine>() != null)
                    {
                        hitCollider.GetComponent<BarrellStateMachine>().Move(owner.velocity * 2f);
                    }
                }
            }
        }

    }

    public override void Update()
    {
        base.Update();

        //Debug.Log("DASH");

        if (jumpForce != LevelManager.instance.dashJumpForce)
        {
            jumpForce = LevelManager.instance.dashJumpForce;
        }

        /*if (timer > dashStateLength && !player.hasFreeDash)
        {
            owner.Transition<WalkState>();

        } else
        {
            timer += Time.deltaTime;
            if (GameManager.instance.dashCooldownImage != null)
            {
                GameManager.instance.dashCooldownImage.fillAmount -= timer / dashStateLength;
            }
        }*/


        if (GameManager.instance.inputManager.JumpKeyDown() && IsGrounded())
        {
            Jump();
            Debug.Log("JUMPING");
            return;
        }
        else if (!GameManager.instance.inputManager.DashKey() || !IsGrounded())
        {
            //player.DashCooldownTimer.Reset();
            owner.Transition<WalkState>();
            Debug.Log("WALKING NOW");
            return;
        }


        //if (owner.velocity.magnitude < player.velocityToDash)
        if(player.DashDurationTimer.IsCompleted(Time.deltaTime, true) && !player.hasFreeDash)
        {
            owner.Transition<WalkState>();
            Debug.Log("TO WALKSTATE");
        } else
        {
            //UIManager.instance.SetDashFillAmountAdd(-dashTimer.GetPercentage());
            UIManager.instance.SetDashFillAmount(1f - player.DashDurationTimer.GetPercentage());
            //Debug.Log("DASHSTATE GETPERCENTAGE: " + -dashTimer.GetPercentage());
        }

        if (Camera.main.fieldOfView <= originalFOV + addToFOV)
        {
           // Camera.main.fieldOfView += fovChangeVelocity * Time.deltaTime;
        }


        

        /*if(owner.velocity.magnitude > player.toSuperDash)
            owner.objectCollider.GetComponent<MeshRenderer>().material.color = Color.red;
    
        else
            owner.objectCollider.GetComponent<MeshRenderer>().material.color = Color.black;*/


        

    }
}
