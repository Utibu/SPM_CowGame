using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBaseState : PhysicsBaseState
{

    public float horizontalPercentage = 0.5f;
    protected Vector3 direction;
    public bool takeInput = true;
    public float jumpForce = 5f;


    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        if (takeInput)
            HandleInput();

        if(!IsGrounded() && owner.GetCurrentState().GetType() != typeof(AirState))
        {
            owner.Transition<AirState>();
        }

        /*
         if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            owner.Transition<JumpState>();
        }
         */

        
    }

    public virtual void Jump()
    {
        Vector3 jumpMovement = Vector2.up * jumpForce * Time.deltaTime;
        owner.velocity += jumpMovement;
        Debug.Log("gravity: " + gravityConstant);
        owner.Transition<AirState>();
    }

    protected void HandleInput()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        //Normalized (direction)
        direction = new Vector3(horizontal, 0f, vertical).normalized;
        direction = (Camera.main.transform.rotation * direction).normalized;
        //direction = new Vector3(direction.x * horizontalPercentage, direction.y, direction.z);
        Vector3 projectedPlane = direction;

        Vector3 normal = GetGroundNormal();
        if (normal != Vector3.zero)
        {
            projectedPlane = Vector3.ProjectOnPlane(direction, normal).normalized;
        }

        //Magnitude (length of movement)
        float distance = acceleration * Time.deltaTime;
        //Move-vector
        Vector3 movement = projectedPlane.normalized * distance;
        //movement = new Vector3(movement.x * horizontalPercentage, movement.y, movement.z);

        //movement = new Vector3(movement.x * horizontalPercentage, movement.y, movement.z);
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            movement *= horizontalPercentage;
        }
        owner.velocity += movement;

        

        if (owner.velocity.magnitude > ((PlayerStateMachine)owner).maxSpeed)
        {
            owner.velocity = owner.velocity.normalized * ((PlayerStateMachine)owner).maxSpeed;
        }
    }

    public override void Leave()
    {
        base.Leave();
    }

    public override void Initialize(StateMachine stateMachine)
    {
        owner = (PlayerStateMachine)stateMachine;
    }

    public override void ActOnTrigger(Collider hitCollider)
    {
        base.ActOnTrigger(hitCollider);
        if (hitCollider.tag.Equals("FragilePlatform"))
        {
            hitCollider.transform.parent.GetComponent<Breakable>().SetFall();
        }

        if (hitCollider.tag.Equals("Button"))
        {
            if (hitCollider.transform.parent.GetComponent<MakeRampButton>() != null && Input.GetKeyDown(KeyCode.E))
            {
                hitCollider.transform.parent.GetComponent<MakeRampButton>().Act();
            }
        }
    }

    public override void ActOnCollision(Collider hitCollider)
    {
        base.ActOnCollision(hitCollider);
        if (hitCollider.tag.Equals("FragilePlatform"))
        {
            //hitCollider.GetComponent<Breakable>().SetFall();
        }

        if(hitCollider.GetComponent<BarrellStateMachine>() != null)
        {
            hitCollider.GetComponent<BarrellStateMachine>().Move(owner.velocity);
        }

        

    }
}
