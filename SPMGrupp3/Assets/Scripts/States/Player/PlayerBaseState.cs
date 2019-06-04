//Main Author: Niklas Almqvist
//Secondary Author: Joakim Ljung, Sofia Kauko
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
        if (((PlayerStateMachine)owner).canTakeInput)
        {
            HandleInput();
            
        }

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

    }

    public virtual void Jump()
    {
        Vector3 jumpMovement = Vector2.up * jumpForce;
        owner.velocity += jumpMovement;
        owner.Transition<AirState>();
        //start jump animation
        player.anim.ResetTrigger("IsLandingTrigger");
        player.anim.SetTrigger("IsJumpingTrigger");
        
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


        if(player.IsDashing && player.UsingFreeCamera == true)
        {
            if(player.VelocityTimer == null)
            {
                player.VelocityTimer = new BasicTimer(3f);
                originalRotation = (player.OriginalCameraRotation * direction).normalized;
                direction = originalRotation;
            } else
            {
                if(player.VelocityTimer.IsCompleted(Time.deltaTime, false, true))
                {
                    player.VelocityTimer = new BasicTimer(3f);
                    originalRotation = (player.OriginalCameraRotation * direction).normalized;
                    direction = originalRotation;
                } else
                {
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
        player.anim.SetFloat("Speed", movement.normalized.magnitude);

        if(player.IsDashing)
        {
            player.anim.SetBool("IsRamming", true);
        } else
        {
            player.anim.SetBool("IsRamming", false);
        }
        //player.anim.SetFloat("Direction", horizontal * speedPercentage);
        direction *= player.CameraRotationSpeed;
        //player.anim.speed = player.animationSpeed;
        //player.meshParent.transform.eulerAngles = new Vector3(0, Camera.main.transform.rotation.eulerAngles.y, 0);
        Vector3 tempDirection = direction;
        tempDirection.y = 0f;
        if (tempDirection.magnitude > 0 && player.IsRotating == false)
        {
            player.RotatePlayer(tempDirection);
        }
        

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
                    BarrellStateMachine barrel = hitCollider.GetComponent<BarrellStateMachine>();
                    if (barrel != null)
                    {
                        barrel.Move(owner.velocity * multiplier);
                        if (GameManager.instance.player.velocity.magnitude > 3f)
                        {
                            EventSystem.Current.FireEvent(new PlaySoundEvent(barrel.transform.position, barrel.GetClip(), 1f, 0.9f, 1.1f));
                        }
                    }
                }
            }
        }
    }

    protected bool CheckDashCooldownCompletion()
    {
        if (player.DashCooldownTimer.IsCompleted(Time.deltaTime, false, false))
        {
            //UIManager.instance.ActivateDashBar();
            
            return true;
        }
        else
        {
            //UIManager.instance.DeactivateDashBar();
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
