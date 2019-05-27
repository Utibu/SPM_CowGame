using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBaseState : PhysicsBaseState
{

    [SerializeField] private float horizontalPercentage = 0.5f;
    [SerializeField] private float diagonalPercentage = 0.8f;
    protected Vector3 direction
    {
        get { return player.Direction; }
        set { player.Direction = value; }
    }
    public bool takeInput = true;
    protected bool canStrafe = true;
    protected float jumpForce = 5f;
    protected float maxSpeed;
    protected PlayerStateMachine player;
    protected float terminalVelocity;
    private float speedPercentage;
    protected bool hasCorrectJump = false;
    private float strafeSpeedReductionPercentage;
    private Vector3 originalRotation;

    public override void Enter()
    {
        base.Enter();
        player.VelocityTimer = null;
    }

    public override void Update()
    {
        base.Update();
        /*
        if (owner.GetCurrentState().GetType() != typeof(DashState))
        {
            CameraPlayerMovement = player.OriginalCameraRotation;
        }
        */
        if (takeInput)
            HandleInput();

        if(!IsGrounded() && owner.GetCurrentState().GetType() != typeof(AirState))
        {
            owner.Transition<AirState>();
        }

        if(CheckDashCooldownCompletion() && GameManager.instance.inputManager.DashKey() && owner.velocity.magnitude > player.velocityToDash)
        {
            //UIManager.instance.ShowSpeedlines();
            player.ShowParticles();
        } else
        {
            //UIManager.instance.HideSpeedlines();
            player.HideParticles();
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
        direction = new Vector3(horizontal, 0f, vertical).normalized;
        //Normalized (direction)
        if (canStrafe)
        {
            
        }
        else
        {
            //direction = new Vector3(horizontal * strafeSpeedReductionPercentage, 0f, vertical).normalized;
        }


        if(owner.GetCurrentState().GetType() == typeof(DashState) && player.DavidCamera == true)
        {
            if(player.VelocityTimer == null)
            {
                //Debug.LogWarning("SETTING INITIAL DASH ROTATION");
                player.VelocityTimer = new BasicTimer(3f);
                //player.IsRotating = false;
                originalRotation = (player.OriginalCameraRotation * direction).normalized;
                direction = originalRotation;
            } else
            {
                if(player.VelocityTimer.IsCompleted(Time.deltaTime, false, true))
                {
                    //Debug.LogWarning("UPDATING DASH ROTATION ");
                    player.VelocityTimer = new BasicTimer(3f);
                    //player.IsRotating = false;
                    originalRotation = (player.OriginalCameraRotation * direction).normalized;
                    direction = originalRotation;
                } else
                {
                    //Debug.Log("UPDATING DASH ROTATION ");
                    direction = Vector3.Lerp(originalRotation, (player.OriginalCameraRotation * direction).normalized, player.VelocityTimer.GetPercentage());
                }
            }
            
        } else
        {
            direction = (player.OriginalCameraRotation * direction).normalized;
        }

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

        float newSpeedPercentage = player.velocity.magnitude / player.maxSpeed;
        speedPercentage = newSpeedPercentage;
        //player.anim.SetFloat("Speed", vertical * speedPercentage);
        //player.anim.SetFloat("Direction", horizontal * speedPercentage);
        direction *= player.CameraRotationSpeed;
        //player.anim.speed = player.animationSpeed;
        //player.meshParent.transform.eulerAngles = new Vector3(0, Camera.main.transform.rotation.eulerAngles.y, 0);
        Vector3 tempDirection = direction;
        tempDirection.y = 0f;
        if (tempDirection.magnitude > 0 && player.IsRotating == false)
        {
            player.RotatePlayer(tempDirection);
        }/*
        if (owner.GetCurrentState().GetType() == typeof(DashState) && player.DavidCamera == true)
        {
            if(Quaternion.LookRotation(tempDirection).eulerAngles.magnitude > 0f)
            {
                player.meshParent.transform.rotation = Quaternion.LookRotation(tempDirection);
                player.IsRotating = false;
                Debug.LogWarning("TAKING OVER ROTATION!!! " + player.meshParent.transform.rotation + "    MAGNITUDE: " + player.meshParent.transform.eulerAngles.magnitude);
            }
            
        } else
        {
            if (tempDirection.magnitude > 0 && player.IsRotating == false)
            {
                player.RotatePlayer(tempDirection);
            }
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
