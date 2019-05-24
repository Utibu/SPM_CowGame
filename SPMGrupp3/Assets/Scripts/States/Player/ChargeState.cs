using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerStateMachine/ChargeState")]
public class ChargeState : PlayerBaseState
{

    //public float velocityToDash;

    public override void Enter()
    {
        base.Enter();
    }

    public override void Leave()
    {
        base.Leave();
    }

    public override void Update()
    {
        base.Update();

        if(jumpForce != LevelManager.instance.normalJumpForce)
        {
            jumpForce = LevelManager.instance.normalJumpForce;
        }
        

        if (GameManager.instance.inputManager.DashKey() && IsGrounded() && owner.velocity.magnitude > player.velocityToDash && (player.allowedToDash || (player.hasFreeDash)))
        {
            owner.Transition<DashState>();
        }

        if (!GameManager.instance.inputManager.DashKey() || !IsGrounded())
        {
            owner.Transition<WalkState>();
        }

        if (GameManager.instance.inputManager.JumpKeyDown() && IsGrounded())
        {
            //owner.Transition<JumpState>();
            Jump();
        }

    }

    public override void ActOnCollision(Collider hitCollider, out bool skipCollision)
    {
        base.ActOnCollision(hitCollider, out skipCollision);
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
                        hitCollider.GetComponent<BarrellStateMachine>().Move(owner.velocity);
                    }
                }
            }
        }

        if (hitCollider.tag.Equals("Enemy"))
        {
            //Destroy(hitCollider.gameObject);
            Peasant bonde = hitCollider.GetComponent<Peasant>();
            bonde.PlayerDash(owner.velocity);
        }
        /*
        if (hitCollider.tag.Equals("Dashable") && player.DashLevel >= hitCollider.GetComponent<Dashable>().requiredLevel)
        {
            if (hitCollider.GetComponent<DroppingObject>() != null)
            {
                //hitCollider.GetComponent<DroppingObject>().OnEnter(player.playerValues);
            }
            Destroy(hitCollider.gameObject);
        }
        */
    }
}
