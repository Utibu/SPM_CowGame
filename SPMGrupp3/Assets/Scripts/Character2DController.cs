using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character2DController : MonoBehaviour
{
    public float acceleration = 2f;
    public LayerMask layerMask;
    public LayerMask movingPlatformMask;
    public float skinWidth = 0.01f;
    BoxCollider2D boxCollider;
    public float gravityConstant = 9.82f;
    public float groundCheckDistance = 0.2f;
    public float jumpForce = 5f;
    Vector2 velocity = Vector2.zero;
    public float staticFrictionForce = 0.6f;
    public float dynamicFrictionPercentage = 0.6f;
    public float airResistance = 0.5f;

    Vector2 characterSize;

    void Awake() {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void DoFriction(float normalForceMagnitude, out Vector2 vel, Vector2 originalVel)
    {
        float staticFriction = normalForceMagnitude * staticFrictionForce;
        vel = originalVel;
        if (velocity.magnitude < staticFriction)
        {
            vel = Vector2.zero;
        } else
        {
            vel += -velocity.normalized * staticFriction * dynamicFrictionPercentage;
        }
    }

    void DoPlatformFriction(float normalForceMagnitude, PhysicObject platform, out Vector2 vel, Vector2 originalVel)
    {
        float playerVelocityOnPlatform = originalVel.magnitude;
        float platformVelocityMagnitude = platform.velocity.magnitude;
        float velocityDiff = playerVelocityOnPlatform - platformVelocityMagnitude;
        float staticFriction = normalForceMagnitude * platform.staticFrictionCoefficient;
        vel = originalVel;
        if (velocityDiff < staticFriction)
        {
            vel = platform.velocity;
        }
        else
        {
            vel += -velocity.normalized * staticFriction * platform.dynamicFrictionPercentage;
        }
    }

    void SetAllowedMovement(float snapChange, int runTimes)
    {
        //Vector2 tempVelocity = velocity;
        bool shouldRun = true;
        do
        {
            runTimes--;
            RaycastHit2D hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0f, velocity.normalized, velocity.magnitude * Time.deltaTime + skinWidth, layerMask);
            if (hit.collider != null)
            {

                if (velocity.magnitude < 0.01f)
                {
                    velocity = Vector2.zero;
                    shouldRun = false;
                    break;
                }

                /*
                    Snapping
                */
                RaycastHit2D normalHit = Physics2D.BoxCast(transform.position, boxCollider.size, 0f, -hit.normal, velocity.magnitude * Time.deltaTime + skinWidth, layerMask);
                Vector2 snapMove = velocity.normalized * (normalHit.distance - skinWidth);
                transform.position += (Vector3)snapMove;
                
                //Save every change in snap (Not working properly right now)
                snapChange += snapMove.magnitude;


                /*
                    Calculate new velocity
                */
                //Find the normal of the collision-object
                Vector2 normal = hit.normal;
                //Get the projection/normal force on the object
                Vector2 projection = Helper.getNormal(velocity, normal);
                //New velocity
                velocity += projection;


                /*
                    Apply friction
                */
                if (hit.collider.gameObject.tag.Equals("MovingPlatform"))
                {
                    PhysicObject platform = hit.collider.GetComponent<PhysicObject>();
                    DoPlatformFriction(projection.magnitude, platform, out velocity, velocity);
                }
                else
                {
                    DoFriction(projection.magnitude, out velocity, velocity);
                }
            }
            else
            {
                shouldRun = false;
            }

            
        } while (shouldRun && runTimes > 0);

        //velocity = velocity.normalized * (velocity.magnitude - snapChange);
        velocity *= Mathf.Pow(airResistance, Time.deltaTime);
        transform.position += (Vector3)velocity * Time.deltaTime;
        
        //velocity = tempVelocity;

    }



    void PreventCollision() {
        SetAllowedMovement(0f, 20);
    }

    bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0f, Vector2.down, groundCheckDistance + skinWidth, layerMask);
        RaycastHit2D platformHit = Physics2D.BoxCast(transform.position, boxCollider.size, 0f, Vector2.down, groundCheckDistance + skinWidth, movingPlatformMask);
        if (hit.collider != null)
        {
            return true;
        } else if(platformHit.collider != null)
        {
            return true;
        }

        return false;
    }

    void GetInput()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        //Normalized (direction)
        Vector2 direction = new Vector2(horizontal, 0f);
        //Magnitude (length of movement)
        float distance = acceleration * Time.deltaTime;
        //Move-vector
        Vector2 movement = direction * distance;
        //Get the allowed magnitude if a collision occurs

        velocity += movement;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            Vector2 jumpMovement = Vector2.up * jumpForce;
            velocity += jumpMovement;
        }


        GetInput();

        Vector2 gravity = Vector2.down * gravityConstant * Time.deltaTime;
        velocity += gravity;

        PreventCollision();
        
    }
}
