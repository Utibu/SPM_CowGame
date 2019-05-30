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

    }

    protected Vector3 GetGroundNormal()
    {
        RaycastHit hit;
        bool ray = Physics.BoxCast(owner.transform.position + owner.objectCollider.center, owner.objectCollider.bounds.extents, Vector3.down, out hit, owner.transform.rotation, float.MaxValue, owner.collisionMask);
        if (ray)
        {
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
        RaycastHit hit;
        bool ray = Physics.BoxCast(owner.transform.position + owner.objectCollider.center, owner.objectCollider.bounds.size / 2, Vector3.down, out hit, owner.transform.rotation, owner.groundCheckDistance + owner.skinWidth, owner.collisionMask);
        if (!ray)
            return false;

        // if ko stands on enemy, CHRUSH
        if (hit.collider.tag.Equals("Enemy"))
        {
            hit.collider.GetComponent<Peasant>().getCrushed();
        }
        return true;

    }

    public Collider GetGroundCollider()
    {
        RaycastHit hit;
        bool ray = Physics.BoxCast(owner.transform.position + owner.objectCollider.center, owner.objectCollider.bounds.size / 2, Vector3.down, out hit, owner.transform.rotation, owner.groundCheckDistance + owner.skinWidth, owner.collisionMask);
        if (!ray)
            return null;
        return hit.collider;
    }

    void CheckTriggers()
    {
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

    //Tack till Vibben och Marcus <3 
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

    
}
