using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerStateMachine/SideDashState")]
public class SideDashState : PlayerBaseState
{

    public float dashDistance = 5f;
    public float exitDashSpeed = 5f;
    private Vector3 inputDirection;
    private float maxDashSpeed;

    public override void Enter()
    {

        base.Enter();
        HandleInput();
        inputDirection = new Vector3(direction.x, 0.0f, direction.z);
        owner.velocity = inputDirection * dashDistance;
        takeInput = false;
        
    }

    public override void Update()
    {
        base.Update();

        Debug.Log(owner.velocity.magnitude);

       

        if (IsGrounded() && owner.velocity.magnitude < exitDashSpeed)
        {
            takeInput = true;
            owner.Transition<WalkState>();
        }
    }

    public override void ActOnCollision(Collider hitCollider)
    {
        base.ActOnCollision(hitCollider);
        if (hitCollider.tag.Equals("Dashable") && owner.velocity.magnitude >= hitCollider.GetComponent<Dashable>().requiredMagnitude)
        {
            if (hitCollider.GetComponent<DroppingObject>() != null)
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

        if (hitCollider.tag.Equals("DashFallable"))
        {
            hitCollider.GetComponent<FallingObject>().SetFalling(owner.velocity.normalized);
        }

        if (hitCollider.tag.Equals("Enemy"))
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

}
