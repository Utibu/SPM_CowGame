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
        player.PlayerSounds.SetPlayerFootstepsSound(FootstepsState.Dash);
        //player.DashCooldownTimer.Reset();
        canStrafe = false;
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

        //player.DashCooldownTimer.Reset();
        /*if(player.DashDurationTimer.GetPercentage() > 0)
        {
            player.DashCooldownTimer.SetStartTime((1 - player.DashDurationTimer.GetPercentage()) * player.DashCooldownTimer.GetDuration());
        } else
        {
            player.DashCooldownTimer.SetStartTime(0);
        }*/
        
        player.DashDurationTimer.Reset();
        canStrafe = true;

        base.Leave();
    }

    public override void ActOnCollision(Collider hitCollider, out bool skipCollision)
    {
        base.ActOnCollision(hitCollider, out skipCollision);

        // calling cam shake from here, so it only shakes on dash collide //
        
        //****************************************************************//

        if(owner.velocity.magnitude < player.velocityToDash)
        {
            return;
        }

        if(hitCollider.GetComponent<Dashable>() != null)
        {
            hitCollider.GetComponent<Dashable>().OnPlayerCollideEnter(hitCollider, out skipCollision, player.DashLevel);
        }

        if (hitCollider.tag.Equals("Enemy"))
        {
            player.ShakeCamera();
            //Destroy(hitCollider.gameObject);
            Peasant bonde = hitCollider.GetComponent<Peasant>();
            bonde.PlayerDash(owner.velocity);
        }
       
        CheckMovableCollision(hitCollider, 2f);

    }

    public override void Update()
    {
        base.Update();

        if (jumpForce != LevelManager.instance.dashJumpForce)
        {
            jumpForce = LevelManager.instance.dashJumpForce;
        }


        if (GameManager.instance.inputManager.JumpKeyDown() && IsGrounded())
        {
            Jump();
            return;
        }
        else if (!GameManager.instance.inputManager.DashKey() || !IsGrounded())
        {
            player.DashCooldownTimer.Reset();
            owner.Transition<WalkState>();
            return;
        }

        if(player.hasFreeDash == false)
        {
            /*if (player.DashDurationTimer.IsCompleted(Time.deltaTime, true))
            {
                owner.Transition<WalkState>();
            }
            else
            {
                UIManager.instance.SetDashFillAmount(1f - player.DashDurationTimer.GetPercentage());
            }*/
        }
        

        if (Camera.main.fieldOfView <= originalFOV + addToFOV)
        {
           // Camera.main.fieldOfView += fovChangeVelocity * Time.deltaTime;
        }


        

    }
}
