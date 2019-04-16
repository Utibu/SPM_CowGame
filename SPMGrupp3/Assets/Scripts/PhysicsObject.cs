using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour
{

    public Vector3 velocity = Vector3.zero;
    //[HideInInspector] public CapsuleCollider playerCollider;
    public LayerMask collisionMask;
    public LayerMask triggerMask;
    public BoxCollider collider;

    public float skinWidth = 0.01f;
    public float groundCheckDistance = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   /* void SetAllowedMovement(float snapChange, int runTimes)
    {
        //Vector3 p1 = owner.transform.position + (Vector3.up * ((owner.playerCollider.height / 2) - owner.playerCollider.radius));
        //Vector3 p2 = owner.transform.position + (Vector3.down * ((owner.playerCollider.height / 2) - owner.playerCollider.radius));
        bool shouldRun = true;
        do
        {
            runTimes--;
            RaycastHit hit;
            //bool ray = Physics.CapsuleCast(p1, p2, owner.playerCollider.radius, owner.velocity.normalized, out hit, float.PositiveInfinity, owner.collisionMask);
            bool ray = Physics.BoxCast(transform.position, collider.bounds.extents, velocity.normalized, out hit, Quaternion.identity, float.PositiveInfinity, collisionMask);
            if (!ray || velocity.magnitude < 0.001f)
            {
                break;
            }
            float angle = (Vector3.Angle(velocity.normalized, hit.normal.normalized) + 90) * Mathf.Deg2Rad;
            float distanceToSnap = hit.distance + skinWidth / Mathf.Sin(angle);

            if (distanceToSnap < velocity.magnitude * Time.deltaTime)
            {
                //Debug.Log("HIT");
                ActOnCollision(hit.collider);
                //Debug.Log(distanceToSnap);
                //No snapping backwards 
                if (distanceToSnap > 0)
                {

                    transform.position += velocity.normalized * distanceToSnap;
                    snapChange += distanceToSnap;
                }

                //Debug.Log("HIT DISTANCE: " + hit.distance);

                Vector3 projection = Helper.getNormal(velocity, hit.normal);

                //New velocity
                velocity += projection;
         
                DoFriction(projection.magnitude, out velocity, velocity);



            }
            else
            {
                break;
            }

        } while (shouldRun && runTimes > 0);

        velocity *= Mathf.Pow(airResistance, Time.deltaTime);
        transform.position += velocity.normalized * (velocity.magnitude * Time.deltaTime - snapChange);

    }*/
}
