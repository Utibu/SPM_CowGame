 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerStateMachine/AirState")]
public class AirState : PlayerBaseState
{

    private float topYPosition;
    public float distanceToTakeFalldamage;
    public float dashJumpAcceleration;
    public float normalJumpAcceleration;
    public float fallDamage;
    private float originalSens;
    [SerializeField] private float divideSens = 10f;
    private bool hasDoneLandingAnimation = false;

    public override void Enter()
    {
        base.Enter();
        originalSens = player.mouseSensitivity;
        hasDoneLandingAnimation = false;
        player.CameraRotationSpeed = 0.8f;
        player.PlayerSounds.SetPlayerFootstepsSound(FootstepsState.None);
        if (player.IsDashing)
        {
            acceleration = dashJumpAcceleration;
            //Debug.Log("ACCELERATION: " + acceleration);
            player.mouseSensitivity /= divideSens;
        } else
        {
            acceleration = normalJumpAcceleration;
        }
            topYPosition = owner.transform.position.y;
    }

    public override void Leave()
    {
        base.Leave();
        player.mouseSensitivity = originalSens;
        //Debug.Log(topYPosition - owner.transform.position.y);
        if (topYPosition - owner.transform.position.y > distanceToTakeFalldamage && Time.time % 60 > 10f)
        {
            //Debug.Log("Falldamage");
            player.playerValues.health -= fallDamage;
        }
    }

    public override void Update()
    {
        base.Update();

        if(topYPosition < owner.transform.position.y)
        {
            topYPosition = owner.transform.position.y;
        }

        if (GameManager.instance.inputManager.DashKey() && IsGrounded())
        {
            //owner.Transition<DashState>();
        }

        if(!GameManager.instance.inputManager.DashKey())
        {
            acceleration = normalJumpAcceleration;
            player.IsDashing = false;
        }

        //Debug.Log("DISTANCE TO GROUND: " + GetDistanceToGround());
        if(GetDistanceToGround() < 2f && hasDoneLandingAnimation == false && topYPosition > owner.transform.position.y)
        {
            player.anim.SetTrigger("IsLandingTrigger");
            player.anim.ResetTrigger("IsJumpingTrigger");
            hasDoneLandingAnimation = true;
        }

        if (IsGrounded())
        {
            owner.Transition<WalkState>();
            //ta-emot animation
            //player.anim.SetBool("IsJumping", false);
            
        }

    }
}
