using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BulletStateMachine/BulletState")]
public class BulletState : PhysicsBaseState
{

    public float moveMultiplier;

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

    }

    public override void Leave()
    {
        base.Leave();
    }

    public override void ActOnCollision(Collider hitCollider, out bool skipCollision)
    {
        if (hitCollider.tag.Equals("Player"))
        {
            //Debug.Log("HIT");
            hitCollider.GetComponent<PlayerValues>().health -= ((BulletStateMachine)owner).bulletDamage;
            hitCollider.GetComponent<PhysicsStateMachine>().velocity = Vector3.zero;
            Destroy(owner.gameObject);
        } else {
            Destroy(owner.gameObject);
        }
        base.ActOnCollision(hitCollider, out skipCollision);
    }
}
