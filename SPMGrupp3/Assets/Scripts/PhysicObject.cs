using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicObject : MonoBehaviour
{
    public Vector2 direction = Vector2.zero;
    public Vector2 velocity = Vector2.zero;
    public Vector2 maxVelocity = Vector2.zero;
    public float acceleration = 1f;
    public float staticFrictionCoefficient = 0.7f;
    public float dynamicFrictionPercentage = 0.6f;
    public BoxCollider2D boxCollider;
    public float skinWidth = 0.01f;
    public LayerMask changeDirectionMask;
    Vector3 objectSize;
    public bool move = true;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        objectSize = GetComponent<Renderer>().bounds.size;
    }

    Vector2 GetAllowedMovement(Vector2 movement, float snapChange)
    {
        RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, objectSize, 0f, movement.normalized, movement.magnitude + skinWidth, changeDirectionMask);
        foreach(RaycastHit2D hit in hits)
        {
            if (hit.collider != null)
            {

                if(hit.collider.gameObject.Equals(this.gameObject))
                {
                    continue;
                }

                if (movement.magnitude < 0.01f)
                {
                    return Vector2.zero;
                }

                Vector2 vel = movement.normalized * (hit.distance - skinWidth);
                direction *= -1;
                float distance = acceleration * Time.deltaTime;
                velocity = direction * distance;
                return vel;
            }
        }
        return movement.normalized * (movement.magnitude - snapChange);

    }


    void PreventCollision()
    {
        Vector2 move = GetAllowedMovement(velocity, 0f);
        transform.position += (Vector3)move;
    }

    // Update is called once per frame
    void Update()
    {
        if(!move)
        {
            return;
        }
        float distance = acceleration * Time.deltaTime;
        Vector2 movement = direction * distance;
        velocity += movement;

        PreventCollision();
        //transform.position += (Vector3)velocity;
    }
}
