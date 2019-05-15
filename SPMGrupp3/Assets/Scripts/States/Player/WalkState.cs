﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerStateMachine/WalkState")]
public class WalkState : PlayerBaseState
{
    public float dashThreshold = 10f;
    [SerializeField] private float dashCooldown = 1f;
    private BasicTimer dashTimer;

    public override void Enter()
    {
        base.Enter();
        dashTimer = new BasicTimer(dashCooldown);
        //UIManager.instance.SetDashFillAmount(0f);
        //dashTimer.Reset();
        Debug.Log("enter walkstate");
        Debug.Log("velocity: " + owner.velocity);
        /*time = 0f;
        if (owner.lastState != null && owner.lastState.GetType() == typeof(DashState))
        {
            allowDash = false;
        }*/
    }

    public override void Leave()
    {
        base.Leave();
        //UIManager.instance.SetDashFillAmount(0f);
    }

    public override void Update()
    {
        if (LevelManager.instance != null && jumpForce != LevelManager.instance.normalJumpForce)
        {
            jumpForce = LevelManager.instance.normalJumpForce;
        }
        /*if(!allowDash)
        {
            time += Time.deltaTime;
            if(time % 60 > dashThreshold)
            {
                allowDash = true;
            }
        }*/

        if (GameManager.instance.inputManager.JumpKeyDown() && IsGrounded())
        {
            //owner.Transition<JumpState>();
            Jump();
        }

        if (GameManager.instance.inputManager.DashKey() && IsGrounded())
        {
            owner.velocity *= 1f;
            //UIManager.instance.SetDashFillAmount(dashTimer.GetPercentage());
            if (player.DashCooldownTimer.IsCompleted(Time.deltaTime, false, false) && dashTimer.IsCompleted(Time.deltaTime, true, true)) // sista villkoret så att man inte kan dasha när man står still.    && owner.velocity.magnitude >= dashThreshold
            {
                owner.Transition<DashState>();
                //dashTimer.Reset();
            }
            
        }

        if (GameManager.instance.inputManager.SideDashKey() && IsGrounded() && player.countdown <= 0)
        {
            //sideDash();
            owner.Transition<SideDashState>();
            player.ResetCooldown();
        }

        /*if (GameManager.instance.dashCooldownImage != null)
        {
            if (player.allowedToDash)
            {
                GameManager.instance.dashCooldownImage.fillAmount = 1;
            }
            else
            {
                GameManager.instance.dashCooldownImage.fillAmount = dashTimer.GetPercentage();
            }

        }*/

        base.Update();

    }
    //TESTSKIT
    /*
    private void sideDash()
    {
        HandleInput();
        inputDirection = new Vector3(direction.x, 0.0f, direction.z);
        Debug.Log(inputDirection);
        owner.velocity = inputDirection * dashDistance * Time.deltaTime;
    }
    */

    public override void ActOnCollision(Collider hitCollider, out bool skipCollision)
    {
        base.ActOnCollision(hitCollider, out skipCollision);
        if (hitCollider.tag.Equals("JumpBale"))
        {
            Collider col = GetGroundCollider();
            if (col != null)
            {
                if(col.tag.Equals("JumpBale"))
                {
                    BarrellStateMachine jumpBale = hitCollider.GetComponent<BarrellStateMachine>();
                    EventSystem.Current.FireEvent(new PlaySoundEvent(jumpBale.gameObject.transform.position, jumpBale.GetClip(), 1f, 0.8f, 1.1f));
                    owner.Transition<JumpBaleState>();

                } else
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
                        hitCollider.GetComponent<BarrellStateMachine>().Move(owner.velocity);
                    }
                }
            }
        }

    }

    public override void ActOnTrigger(Collider hitCollider)
    {
        base.ActOnTrigger(hitCollider);
        Interactable interactable = hitCollider.GetComponent<Interactable>();
        if (interactable != null)
        {
            //Show E-button on screen
            UIManager.instance.ShowInteractionIndicator();
            player.IsWithinTriggerRange = true;
            if(GameManager.instance.inputManager.EventKeyDown())
            {
                owner.Transition<InteractState>();
                interactable.InvokeInteraction();
            }
            
        }
    }

    

}
