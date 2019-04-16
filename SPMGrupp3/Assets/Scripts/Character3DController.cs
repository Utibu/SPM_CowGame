using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character3DController : MonoBehaviour
{
    Vector3 velocity = Vector3.zero;
    public float acceleration = 2f;
    CapsuleCollider playerCollider;
    public float skinWidth = 0.01f;
    public LayerMask layerMask;
    public float staticFrictionForce = 0.6f;
    public float dynamicFrictionPercentage = 0.6f;
    public float airResistance = 0.5f;
    public float gravityConstant = 2f;
    public float jumpForce = 5f;
    public float groundCheckDistance = 0.2f;
    public float mouseSensitivity;
    float rotationX;
    float rotationY;
    public float minAngle;
    public float maxAngle;
    public Vector3 cameraPositionRelativeToPlayer;
    public SphereCollider cameraCollider;

    private void Awake()
    {
        playerCollider = GetComponent<CapsuleCollider>();
        cameraCollider = Camera.main.GetComponent<SphereCollider>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            Vector3 jumpMovement = Vector2.up * jumpForce;
            velocity += jumpMovement;
        }

        HandleInput();

        HandleCamera();

        ApplyGravity();

        PreventCollision();

        //transform.position += velocity;
    }

    void DoFriction(float normalForceMagnitude, out Vector3 vel, Vector3 originalVel)
    {
        float staticFriction = normalForceMagnitude * staticFrictionForce;
        vel = originalVel;
        if (velocity.magnitude < staticFriction)
        {
            vel = Vector3.zero;
        } else
        {
            vel += -velocity.normalized * staticFriction * dynamicFrictionPercentage;
        }
    }

    /*Vector3 GetAllowedMovement(float snapChange, LayerMask mask)
    {
        //RaycastHit hit = Physics.CapsuleCast(transform.position, playerCollider.bounds.size, 0f, velocity.normalized, velocity.magnitude + skinWidth, mask);
        Vector3 p1 = transform.position + (Vector3.up * ((playerCollider.height / 2) - playerCollider.radius));
        Vector3 p2 = transform.position + (Vector3.down * ((playerCollider.height / 2) - playerCollider.radius));

        //Tagen från dokumentationen 
        //Vector3 p1 = transform.position + playerCollider.center + Vector3.up * -playerCollider.height * 0.5F;
        //Vector3 p2 = p1 + Vector3.up * playerCollider.height;

        //Vector3 p1 = transform.position + Vector3.up * -playerCollider.height * 0.5f;
        //Vector3 p2 = p1 + Vector3.up * playerCollider.height;

        //RaycastHit[] t = Physics.CapsuleCastAll(p1, p2, playerCollider.radius, velocity.normalized, velocity.magnitude + skinWidth, mask);
        RaycastHit[] t = Physics.BoxCastAll(transform.position, playerCollider.bounds.size / 2, velocity.normalized, Quaternion.identity, velocity.magnitude + skinWidth, mask);
        Debug.Log(t.Length);

        RaycastHit hit;
        //bool ray = Physics.CapsuleCast(p1, p2, playerCollider.radius, velocity.normalized, out hit, velocity.magnitude + skinWidth, mask);
        bool ray = Physics.BoxCast(transform.position, playerCollider.bounds.size / 2, velocity.normalized, out hit, Quaternion.identity, velocity.magnitude + skinWidth, mask);
        if (ray) {
            //Debug.Log("FHIT " + hit.collider.name);
            if (hit.collider != null)
            {

                //Debug.Log("HIT");

                if (velocity.magnitude < 0.01f)
                {
                    return Vector3.zero;
                }

                //RaycastHit2D normalHit = Physics2D.BoxCast(transform.position, boxCollider.size, 0f, -hit.normal, movement.magnitude + skinWidth, mask);
                RaycastHit normalHit;
                bool okSecondHit = Physics.CapsuleCast(p1, p2, playerCollider.radius, -hit.normal, out normalHit, velocity.magnitude + skinWidth, mask);

                //Debug.Log("Normalhit!");
                //Find the normal of the collision-object
                Vector3 normal = hit.normal;
                //Calculate how much to move to snap
                Vector3 vel = velocity.normalized * (normalHit.distance - skinWidth);
                //Get the projection/normal force on the object
                Vector3 projection = Helper.getNormal(velocity, normal);

                velocity = velocity + projection;

                //DoFriction(projection.magnitude);

                transform.position += (Vector3)vel;

                // trabsforn.position ska flyttas med vektorn mellan de röda punkterns
                // Sen göra om med den blåa vektorn

                return GetAllowedMovement(0f, mask);

            }
        }

        

        return velocity.normalized * (velocity.magnitude - snapChange);
    }*/


    /*
        void PreventCollision()
        {
            Debug.LogWarning("START PREVENT!!!!");
            Vector3 move = GetAllowedMovement(0f, layerMask);
            velocity *= Mathf.Pow(airResistance, Time.deltaTime);
            transform.position += (Vector3)move;

            //MovingPlatformCollision();
        }*/



    /*
     * OLD VERSION BUT WORKS BETTER 
     */
    void SetAllowedMovement(float snapChange, int runTimes)
    {
        //Vector2 tempVelocity = velocity;
        Vector3 p1 = transform.position + (Vector3.up * ((playerCollider.height / 2) - playerCollider.radius));
        Vector3 p2 = transform.position + (Vector3.down * ((playerCollider.height / 2) - playerCollider.radius));
        bool shouldRun = true;
        do
        {
            RaycastHit hit;
            bool ray = Physics.CapsuleCast(p1, p2, playerCollider.radius, velocity.normalized, out hit, velocity.magnitude + skinWidth, layerMask);
            //bool ray = Physics.BoxCast(transform.position, playerCollider.bounds.size / 2, velocity.normalized, out hit, Quaternion.identity, velocity.magnitude + skinWidth, layerMask);
            if (ray)
            {
                //Debug.Log("FHIT " + hit.collider.name);
                if (hit.collider != null)
                {

                    if (velocity.magnitude < 0.01f)
                    {
                        velocity = Vector2.zero;
                        shouldRun = false;
                        break;
                    }

                    RaycastHit normalHit;
                    bool okSecondHit = Physics.CapsuleCast(p1, p2, playerCollider.radius, -hit.normal, out normalHit, velocity.magnitude + skinWidth, layerMask);

                    /*
                        Snapping
                    */
                    Vector3 snapMove = velocity.normalized * (normalHit.distance - skinWidth);
                    transform.position += snapMove;
                    //Save every change in snap (Not working properly right now)
                    snapChange += snapMove.magnitude;


                    /*
                        Calculate new velocity
                    */
                    //Find the normal of the collision-object
                    Vector3 normal = hit.normal;
                    //Get the projection/normal force on the object
                    Vector3 projection = Helper.getNormal(velocity, normal);
                    //New velocity
                    velocity += projection;


                    /*
                        Apply friction
                    */
                    DoFriction(projection.magnitude, out velocity, velocity);
                }
                else
                {
                    shouldRun = false;
                }


            }
            else
            {
                shouldRun = false;
            }

            runTimes--;
        } while (shouldRun && runTimes > 0);

       // velocity = velocity.normalized * (velocity.magnitude - snapChange);
        velocity *= Mathf.Pow(airResistance, Time.deltaTime);
        transform.position += velocity;

    }

    bool IsGrounded()
    {
        Vector3 p1 = transform.position + (Vector3.up * ((playerCollider.height / 2) - playerCollider.radius));
        Vector3 p2 = transform.position + (Vector3.down * ((playerCollider.height / 2) - playerCollider.radius));
        RaycastHit hit;
        bool ray = Physics.CapsuleCast(p1, p2, playerCollider.radius, Vector3.down, out hit, groundCheckDistance + skinWidth, layerMask);
        if(ray) {
            if (hit.collider != null)
            {
                return true;
            }
        }
        return false;
    }

    Vector3 GetGroundNormal() {
        Vector3 p1 = transform.position + (Vector3.up * ((playerCollider.height / 2) - playerCollider.radius));
        Vector3 p2 = transform.position + (Vector3.down * ((playerCollider.height / 2) - playerCollider.radius));
        RaycastHit hit;
        bool ray = Physics.CapsuleCast(p1, p2, playerCollider.radius, Vector3.down, out hit, groundCheckDistance + skinWidth, layerMask);
        if(ray) {
            if (hit.collider != null)
            {
                return hit.normal;
            }
        }
        return Vector3.zero;
    }

    Vector3 GetAllowedCameraMovement(Vector3 goalVector) {
        RaycastHit hit;
        //bool okHit = Physics.SphereCast(transform.position, cameraCollider.radius, -cameraPositionRelativeToPlayer.normalized, out hit, cameraPositionRelativeToPlayer.magnitude, layerMask);
        bool okHit = Physics.SphereCast(transform.position, cameraCollider.radius, goalVector.normalized, out hit, goalVector.magnitude, layerMask);
        if(okHit) {
            if(hit.collider != null) {
                //Vector3 allowedMovement = cameraPositionRelativeToPlayer.normalized * (hit.distance - cameraCollider.radius);
                Vector3 allowedMovement = goalVector.normalized * (hit.distance - cameraCollider.radius);
                return allowedMovement;
            }
        }

        return goalVector;
    }

    void PreventCollision() {
        SetAllowedMovement(0f, 20);
    }

    void ApplyGravity()
    {
        Vector3 gravity = Vector3.down * gravityConstant * Time.deltaTime;
        velocity += gravity;
    }

    void HandleInput()
    {
        //velocity = Vector3.zero;
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        //Normalized (direction)
        Vector3 direction = new Vector3(horizontal, 0f, vertical);
        direction = Camera.main.transform.rotation * direction;
        Vector3 projectedPlane = direction;

        Vector3 normal = GetGroundNormal();
        if(normal != Vector3.zero) {
            projectedPlane = Vector3.ProjectOnPlane(direction, normal).normalized;
        }

        //Magnitude (length of movement)
        float distance = acceleration * Time.deltaTime;
        //Move-vector
        Vector3 movement = projectedPlane.normalized * distance;
        //Get the allowed magnitude if a collision occurs
        velocity += movement;
    }

    /*
     First person
     */
    void HandleCameraFirstPerson()
    {
        float horizontal = Input.GetAxisRaw("Mouse X");
        float vertical = Input.GetAxisRaw("Mouse Y");

        rotationX -= vertical * mouseSensitivity;
        rotationY += horizontal * mouseSensitivity;

        rotationX = Mathf.Clamp(rotationX, minAngle, maxAngle);

        Camera.main.transform.rotation = Quaternion.Euler(rotationX, rotationY, 0f);

    }

    void HandleCamera()
    {
        float horizontal = Input.GetAxisRaw("Mouse X");
        float vertical = Input.GetAxisRaw("Mouse Y");

        rotationX -= vertical * mouseSensitivity;
        rotationY += horizontal * mouseSensitivity;

        rotationX = Mathf.Clamp(rotationX, minAngle, maxAngle);

        Camera.main.transform.rotation = Quaternion.Euler(rotationX, rotationY, 0f);

        Vector3 cameraPlayerRelationship = Camera.main.transform.rotation * cameraPositionRelativeToPlayer;
        Vector3 okToMove = GetAllowedCameraMovement(cameraPlayerRelationship);
        Camera.main.transform.position = transform.position + okToMove;

    }
}
