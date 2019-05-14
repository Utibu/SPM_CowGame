using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBaseState : PhysicsBaseState
{

    public float horizontalPercentage = 0.5f;
    protected Vector3 direction;
    public bool takeInput = true;
    protected float jumpForce = 5f;
    protected float maxSpeed;
    protected PlayerStateMachine player;
    protected float terminalVelocity;
    private float speedPercentage;
    protected bool hasCorrectJump = false;


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

        

        //terminalVelocity = ((player.dashStateGravity * Time.deltaTime) + (player.dashStateAcceleration * Time.deltaTime) - normalForce.magnitude) / (1 - Mathf.Pow(player.dashAirResistance, Time.deltaTime));
        //player.terminalVelocity = terminalVelocity;
        //Debug.Log(terminalVelocity);

        /*
         if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            owner.Transition<JumpState>();
        }
         */


    }

    public virtual void Jump()
    {
        Vector3 jumpMovement = Vector2.up * jumpForce;
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
        if (horizontal != 0)
        {
            movement *= horizontalPercentage;
        } else if(vertical < 0)
        {
            movement *= horizontalPercentage;
        }

        /*player.anim.SetFloat("Speed", vertical);
        player.anim.SetFloat("Direction", horizontal);*/
        float newSpeedPercentage = player.velocity.magnitude / player.maxSpeed;
        /*if(Mathf.Abs(newSpeedPercentage - speedPercentage) > 0.1f)
        {
            speedPercentage = newSpeedPercentage;
        }*/
        speedPercentage = newSpeedPercentage;
        player.anim.SetFloat("Speed", vertical * speedPercentage);
        player.anim.SetFloat("Direction", horizontal * speedPercentage);
        player.anim.speed = player.animationSpeed;
        //player.transform.rotation = Quaternion.Euler((Camera.main.transform.rotation * direction).normalized);
        player.meshParent.transform.eulerAngles = new Vector3(0, Camera.main.transform.rotation.eulerAngles.y, 0);

        /*if (maxSpeed <= 0)
        {
            maxSpeed = player.maxSpeed;
        }
        if (movement.magnitude > maxSpeed)
        {
            movement = movement.normalized * maxSpeed;
        }*/

        owner.velocity += movement;

        
        
    }

    public override void Leave()
    {
        base.Leave();
    }

    public override void Initialize(StateMachine stateMachine)
    {
        owner = (PhysicsStateMachine)stateMachine;
        player = (PlayerStateMachine)stateMachine;
    }

    public override void ActOnTrigger(Collider hitCollider)
    {
        base.ActOnTrigger(hitCollider);

        if (hitCollider.tag.Equals("Droppable"))
        {
            hitCollider.GetComponent<DroppableObject>().OnEnter();
        }

        if (hitCollider.tag.Equals("Checkpoint"))
        {
            LevelManager.instance.RegisterCheckpointTaken(hitCollider.transform);
        }

        if (hitCollider.tag.Equals("FragilePlatform"))
        {
            hitCollider.transform.parent.GetComponent<Breakable>().SetFall();
        }
        
        if (hitCollider.tag.Equals("Button"))
        {
            if (hitCollider.transform.GetComponent<ButtonScript>() != null && GameManager.instance.inputManager.EventKeyDown())
            {
                hitCollider.transform.GetComponent<ButtonScript>().Act();
            }
        }

        if (hitCollider.GetComponent<LoadScene>() != null)
        {
            GameManager.instance.LoadScene(hitCollider.GetComponent<LoadScene>().sceneIndex);
        } 

        if(hitCollider.tag.Equals("Killzone"))
        {
            GameManager.instance.player.playerValues.Die();
        }
        

        if (hitCollider.GetComponent<BaseTrigger>() != null) {
            hitCollider.GetComponent<BaseTrigger>().OnTriggerEnter();
        }

        PlayerSounds playerSounds = player.GetComponent<PlayerSounds>();
        if (playerSounds != null)
        {
            playerSounds.PlayTriggerSound(hitCollider);
        }

        
    }

    public override void ActOnCollision(Collider hitCollider, out bool skipCollision)
    {
        base.ActOnCollision(hitCollider, out skipCollision);
        if (hitCollider.tag.Equals("FragilePlatform"))
        {
            //hitCollider.GetComponent<Breakable>().SetFall();
        }

        if (hitCollider.tag.Equals("Key"))
        {
            owner.GetComponentInParent<PlayerValues>().gotKey1 = true;
            Debug.Log("you have a key (actOnCollision)");
            GameObject key = hitCollider.transform.gameObject;
            Destroy(key);
        }

        if (hitCollider.tag.Equals("Gate1") && owner.GetComponent<PlayerValues>().gotKey1)
        {
            hitCollider.GetComponentInParent<GateScript>().Open();
        }

        PlayerSounds playerSounds = player.GetComponent<PlayerSounds>();
        if (playerSounds != null)
        {
            playerSounds.PlayCollisionSound(hitCollider);
        }

        //if(hitCollider.tag.Equals("Rock")) {
        //Destroy(hitCollider.gameObject);
        //}


    }
}
