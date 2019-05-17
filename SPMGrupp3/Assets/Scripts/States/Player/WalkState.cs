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

        //owner.velocity.magnitude >= dashThreshold
        if (CheckDashCooldownCompletion() && GameManager.instance.inputManager.DashKey() && IsGrounded() && dashTimer.IsCompleted(Time.deltaTime, true, true))
        {
            owner.Transition<DashState>();
        }

        /*if (GameManager.instance.inputManager.SideDashKey() && IsGrounded() && player.countdown <= 0)
        {
            //sideDash();
            owner.Transition<SideDashState>();
            player.ResetCooldown();
        }*/

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
        CheckMovableCollision(hitCollider, 1f);

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
