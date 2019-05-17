using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsBaseState : State
{
    [SerializeField] protected float acceleration;
    [SerializeField] protected float airResistance = 0.5f;
    [SerializeField] protected float gravityConstant = 2f;

    
    [SerializeField] protected float staticFrictionForce = 0.6f;
    [SerializeField] protected float dynamicFrictionPercentage = 0.6f;

    //public float terminalVelocity;
    protected Vector3 normalForce;
    protected float currentFriction;

    protected PhysicsStateMachine owner;

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        normalForce = Vector3.zero;

        ApplyGravity();

        PreventCollision();
        CheckTriggers();

        
        
        //Debug.Log("GRAVITY: " + gravityConstant);
        //Debug.Log(owner.velocity.magnitude);

    }

    protected Vector3 GetGroundNormal()
    {
        //Vector3 p1 = owner.transform.position + (Vector3.up * ((owner.objectCollider.height / 2) - owner.objectCollider.radius));
        //Vector3 p2 = owner.transform.position + (Vector3.down * ((owner.objectCollider.height / 2) - owner.objectCollider.radius));

        //owner.groundCheckDistance + owner.skinWidth
        RaycastHit hit;
        bool ray = Physics.BoxCast(owner.transform.position + owner.objectCollider.center, owner.objectCollider.bounds.extents, Vector3.down, out hit, owner.transform.rotation, float.MaxValue, owner.collisionMask);
        if (ray)
        {
            //Debug.Log("RAY");
            if (hit.collider != null)
            {
                return hit.normal;
            }
        }
        return Vector3.zero;
    }

    protected void PreventCollision()
    {
        CollisionCheck(owner.velocity * Time.deltaTime);
        owner.velocity *= Mathf.Pow(airResistance, Time.deltaTime);
        //SetAllowedMovement(0f, 10);
    }

    protected void ApplyGravity()
    {
        Vector3 gravity = Vector3.down * gravityConstant * Time.deltaTime;
        owner.velocity += gravity;
    }

    void DoFriction(float normalForceMagnitude, out Vector3 vel, Vector3 originalVel)
    {
        float staticFriction = normalForceMagnitude * staticFrictionForce;
        //currentFriction = (normalForce.normalized * staticFrictionForce * dynamicFrictionPercentage).magnitude;
        vel = originalVel;
        if (owner.velocity.magnitude < staticFriction)
        {
            vel = Vector3.zero;
        }
        else
        {
            vel += -owner.velocity.normalized * staticFriction * dynamicFrictionPercentage;
        }
    }

    public override void Leave()
    {
        base.Leave();
    }

    public override void Initialize(StateMachine stateMachine)
    {
        owner = (PhysicsStateMachine)stateMachine;
    }

    public bool IsGrounded()
    {
        //Vector3 p1 = owner.transform.position + (Vector3.up * ((owner.objectCollider.height / 2) - owner.objectCollider.radius));
        //Vector3 p2 = owner.transform.position + (Vector3.down * ((owner.objectCollider.height / 2) - owner.objectCollider.radius));
        //owner.groundCheckDistance + owner.skinWidth
        /*RaycastHit hit;
        bool ray = Physics.BoxCast(owner.transform.position + owner.objectCollider.center, owner.objectCollider.bounds.size / 2, Vector3.down, out hit, owner.transform.rotation, float.MaxValue, owner.collisionMask);
        if (!ray)
        {
            return false;
        }
        Debug.Log("DISTANCE: " + hit.distance);
        Debug.Log("MAXDISTANCE: " + (owner.groundCheckDistance + owner.skinWidth + owner.objectCollider.center.magnitude));
        Debug.Log("HIT POINT: " + hit.point);
        if(hit.distance < owner.groundCheckDistance + owner.skinWidth + owner.objectCollider.center.magnitude)
        {
            return true;
        }
        return false;*/

        RaycastHit hit;
        bool ray = Physics.BoxCast(owner.transform.position + owner.objectCollider.center, owner.objectCollider.bounds.size / 2, Vector3.down, out hit, owner.transform.rotation, owner.groundCheckDistance + owner.skinWidth, owner.collisionMask);
        if (!ray)
            return false;
        return true;

    }

    public Collider GetGroundCollider()
    {
        //Vector3 p1 = owner.transform.position + (Vector3.up * ((owner.objectCollider.height / 2) - owner.objectCollider.radius));
        //Vector3 p2 = owner.transform.position + (Vector3.down * ((owner.objectCollider.height / 2) - owner.objectCollider.radius));
        RaycastHit hit;
        bool ray = Physics.BoxCast(owner.transform.position + owner.objectCollider.center, owner.objectCollider.bounds.size / 2, Vector3.down, out hit, owner.transform.rotation, owner.groundCheckDistance + owner.skinWidth, owner.collisionMask);
        if (!ray)
            return null;
        return hit.collider;
    }

    void CheckTriggers()
    {
        //Vector3 p1 = owner.transform.position + (Vector3.up * ((owner.objectCollider.height / 2) - owner.objectCollider.radius));
        // Vector3 p2 = owner.transform.position + (Vector3.down * ((owner.objectCollider.height / 2) - owner.objectCollider.radius));
        Vector3 dir = owner.velocity.normalized;
        if (owner.velocity.magnitude <= 0f)
        {
            dir = owner.transform.forward;
        }
        RaycastHit[] hits = Physics.BoxCastAll(owner.transform.position + owner.objectCollider.center, owner.objectCollider.bounds.size / 2, dir, owner.transform.rotation, 0.2f, owner.triggerMask);
        foreach (RaycastHit hit in hits)
        {
            //Debug.Log("hit");
            ActOnTrigger(hit.collider);
        }
    }

    public virtual void ActOnTrigger(Collider hitCollider)
    {
       
    }

    public virtual void ActOnCollision(Collider hitCollider, out bool skipCollision)
    {
        skipCollision = false;
    }

    //Tack till Vibben och Markus <3 
    public void CollisionCheck(Vector3 frameMovement)
    {
        RaycastHit hit;
        if (Physics.BoxCast(owner.transform.position + owner.objectCollider.center, owner.objectCollider.bounds.extents, frameMovement.normalized, out hit, owner.transform.rotation, float.PositiveInfinity, owner.collisionMask))
        {

            float angle = (Vector3.Angle(hit.normal, frameMovement.normalized) - 90) * Mathf.Deg2Rad;
            float snapDistanceFromHit = owner.skinWidth / Mathf.Sin(angle);

            

            Vector3 snapMovementVector = frameMovement.normalized * (hit.distance - snapDistanceFromHit);
            snapMovementVector = Vector3.ClampMagnitude(snapMovementVector, frameMovement.magnitude);

            owner.transform.position += snapMovementVector;
            
            frameMovement -= snapMovementVector;

            Vector3 frameMovementNormalForce = Helper.getNormal(frameMovement, hit.normal);
            frameMovement += frameMovementNormalForce;

            if (frameMovementNormalForce.magnitude > 0.001f)
            {
                bool skipCollision = false;
                ActOnCollision(hit.collider, out skipCollision);

                if(skipCollision == false)
                {
                    Vector3 velocityNormalForce = Helper.getNormal(owner.velocity, hit.normal);
                    owner.velocity += velocityNormalForce;
                    DoFriction(velocityNormalForce.magnitude, out owner.velocity, owner.velocity);
                }
            }

            if (frameMovement.magnitude > 0.001f)
            {
                CollisionCheck(frameMovement);
            }
            return;
        }

        else
        {
            owner.transform.position += frameMovement;
        }
    }

    void SetAllowedMovement(float snapChange, int runTimes)
    {
        //Vector3 p1 = owner.transform.position + (Vector3.up * ((owner.objectCollider.height / 2) - owner.objectCollider.radius));
        //Vector3 p2 = owner.transform.position + (Vector3.down * ((owner.objectCollider.height / 2) - owner.objectCollider.radius));
        bool shouldRun = true;
        do
        {
            runTimes--;
            RaycastHit hit;
            //bool ray = Physics.CapsuleCast(p1, p2, owner.objectCollider.radius, owner.velocity.normalized, out hit, float.PositiveInfinity, owner.collisionMask);
            bool ray = Physics.BoxCast(owner.transform.position + owner.objectCollider.center, owner.objectCollider.bounds.extents, owner.velocity.normalized, out hit, Quaternion.identity, float.PositiveInfinity, owner.collisionMask);
            if (!ray || owner.velocity.magnitude < 0.001f)
            {
                break;
            }
            float angle = (Vector3.Angle(owner.velocity.normalized, hit.normal.normalized) + 90) * Mathf.Deg2Rad;
            float distanceToSnap = hit.distance + owner.skinWidth / Mathf.Sin(angle);

            if (distanceToSnap < owner.velocity.magnitude * Time.deltaTime)
            {
                //Debug.Log("HIT");
                bool skipCollision = false;
                ActOnCollision(hit.collider, out skipCollision);

                if(skipCollision)
                {
                    continue;
                }
                //Debug.Log(distanceToSnap);
                //No snapping backwards 
                if (distanceToSnap > 0)
                {

                    owner.transform.position += owner.velocity.normalized * distanceToSnap;
                    snapChange += distanceToSnap;
                }

                //Debug.Log("HIT DISTANCE: " + hit.distance);

                /*
                    Calculate new velocity
                */
                Vector3 projection = Helper.getNormal(owner.velocity, hit.normal);
                normalForce += projection;

                Debug.DrawLine(owner.transform.position, owner.transform.position + (owner.velocity + projection), Color.red);
                Debug.DrawLine(hit.point, hit.point + hit.normal, Color.blue);
                Debug.DrawLine(hit.point, hit.point + projection * 10f, Color.magenta);
                Debug.DrawLine(hit.point, hit.point + (hit.normal.normalized * hit.distance), Color.green);

                //NOTE: Debugs
                /* Debug.DrawLine(owner.transform.position, owner.transform.position + (owner.velocity.normalized * distanceToSnap), Color.yellow);
                Debug.DrawLine(owner.transform.position, owner.transform.position + (owner.velocity * Time.deltaTime), Color.blue);
                Debug.DrawLine(hit.point, hit.point + hit.normal, Color.red);
                Debug.DrawLine(owner.transform.position, hit.point, Color.green);
                hit.collider.gameObject.GetComponent<MeshRenderer>().material.color = Color.red; */

                //New velocity
                owner.velocity += projection;
                Debug.DrawLine(owner.transform.position, owner.transform.position + owner.velocity, Color.black);

                /*
                    Apply friction
                */
                DoFriction(projection.magnitude, out owner.velocity, owner.velocity);



            }
            else
            {
                break;
            }

        } while (shouldRun && runTimes > 0);

        if(owner.tag.Equals("Player"))
        {
            /*
            Debug.Log("Velocity before: " + owner.velocity.magnitude);
            Debug.Log("Air resistance: " + airResistance);
            Debug.Log("Acceleration: " + acceleration);
            Debug.Log("Gravity: " + gravityConstant);
            Debug.Log("Deltatime: " + Time.deltaTime);
            */
        }
        
        owner.velocity *= Mathf.Pow(airResistance, Time.deltaTime);

        if (owner.tag.Equals("Player"))
        {
            /*
            Debug.Log("POW: " + Mathf.Pow(airResistance, Time.deltaTime));
            Debug.Log("Velocity after: " + owner.velocity.magnitude);
            */
        }

        owner.transform.position += owner.velocity.normalized * (owner.velocity.magnitude * Time.deltaTime - snapChange);

    }
}
