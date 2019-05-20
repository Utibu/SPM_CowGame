using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBaseState : PhysicsBaseState
{

    [SerializeField] private float horizontalPercentage = 0.5f;
    [SerializeField] private float diagonalPercentage = 0.8f;
    protected Vector3 direction;
    public bool takeInput = true;
    protected bool canStrafe = true;
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
        owner.Transition<AirState>();
    }

    

    protected void HandleInput()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        //Normalized (direction)
        if (canStrafe)
        {
            direction = new Vector3(horizontal, 0f, vertical).normalized;
        }
        else
        {
            direction = new Vector3(horizontal * 0.4f, 0f, vertical).normalized;
        }
        //direction = (Camera.main.transform.rotation * direction).normalized;
        direction = (player.OriginalCameraRotation * direction).normalized;
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
        if (vertical > 0 && horizontal != 0)
        {
            //Debug.Log("diagonal");
            //movement *= diagonalPercentage;
        }

        else if (horizontal != 0 || vertical < 0)
        {
            //Debug.Log("Horizontal");
            //movement *= horizontalPercentage;

        }

        /*player.anim.SetFloat("Speed", vertical);
        player.anim.SetFloat("Direction", horizontal);*/
        float newSpeedPercentage = player.velocity.magnitude / player.maxSpeed;
        /*if(Mathf.Abs(newSpeedPercentage - speedPercentage) > 0.1f)
        {
            speedPercentage = newSpeedPercentage;
        }*/
        speedPercentage = newSpeedPercentage;
        //player.anim.SetFloat("Speed", vertical * speedPercentage);
        //player.anim.SetFloat("Direction", horizontal * speedPercentage);
        //player.anim.speed = player.animationSpeed;
        //player.meshParent.transform.eulerAngles = new Vector3(0, Camera.main.transform.rotation.eulerAngles.y, 0);
        Vector3 tempDirection = direction;
        tempDirection.y = 0f;
        if(tempDirection.magnitude > 0 && player.IsRotating == false)
        {
            player.RotatePlayer(tempDirection);

            //player.meshParent.transform.rotation = Quaternion.LookRotation(tempVelocity);
        }
        
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

        if(hitCollider.GetComponent<Triggable>() != null)
        {
            hitCollider.GetComponent<Triggable>().OnPlayerTriggerEnter(hitCollider);
        }

        if (hitCollider.GetComponent<LoadScene>() != null)
        {
            GameManager.instance.LoadScene(hitCollider.GetComponent<LoadScene>().sceneIndex);
        } 

        if(hitCollider.tag.Equals("Killzone"))
        {
            GameManager.instance.player.playerValues.Die();
        }

        PlayerSounds playerSounds = player.GetComponent<PlayerSounds>();
        if (playerSounds != null)
        {
            playerSounds.PlayTriggerSound(hitCollider);
        }

        
    }

    protected void CheckMovableCollision(Collider hitCollider, float multiplier = 1f)
    {
        if (hitCollider.tag.Equals("JumpBale"))
        {
            Collider col = GetGroundCollider();
            if (col != null)
            {
                if (col.tag.Equals("JumpBale"))
                {
                    BarrellStateMachine jumpBale = hitCollider.GetComponent<BarrellStateMachine>();
                    EventSystem.Current.FireEvent(new PlaySoundEvent(jumpBale.gameObject.transform.position, jumpBale.GetClip(), 1f, 0.8f, 1.1f));
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
                        hitCollider.GetComponent<BarrellStateMachine>().Move(owner.velocity * multiplier);
                    }
                }
            }
        }
    }

    protected bool CheckDashCooldownCompletion()
    {
        if (player.DashCooldownTimer.IsCompleted(Time.deltaTime, false, false))
        {
            UIManager.instance.ActivateDashBar();
            
            return true;
        }
        else
        {
            UIManager.instance.DeactivateDashBar();
            return false;
        }
    }

    public override void ActOnCollision(Collider hitCollider, out bool skipCollision)
    {
        base.ActOnCollision(hitCollider, out skipCollision);
        
        if(hitCollider.GetComponent<Collidable>() != null)
        {
            hitCollider.GetComponent<Collidable>().OnPlayerCollideEnter(hitCollider, out skipCollision);
        }
        /*
        if (hitCollider.tag.Equals("FragilePlatform"))
        {
            //hitCollider.GetComponent<Breakable>().SetFall();
        }
        */
        
        /*
        PlayerSounds playerSounds = player.GetComponent<PlayerSounds>();
        if (playerSounds != null)
        {
            playerSounds.PlayCollisionSound(hitCollider);
        }
        */
        //if(hitCollider.tag.Equals("Rock")) {
        //Destroy(hitCollider.gameObject);
        //}


    }
}
