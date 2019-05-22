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

    public override void Enter()
    {
        base.Enter();
        originalSens = player.mouseSensitivity;
        player.PlayerSounds.SetPlayerFootstepsSound(FootstepsState.None);
        if (owner.lastState.GetType() == typeof(DashState))
        {
            acceleration = dashJumpAcceleration;
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

        if (IsGrounded())
        {
            owner.Transition<WalkState>();
        }

    }
}
