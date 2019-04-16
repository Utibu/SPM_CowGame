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

    public override void ActOnCollision(Collider hitCollider)
    {
        if (hitCollider.tag.Equals("Player"))
        {
            //Debug.Log("HIT");
            hitCollider.GetComponent<PlayerValues>().health -= 10;
            hitCollider.GetComponent<PhysicsStateMachine>().velocity = Vector3.zero;
            Destroy(owner.gameObject);
        } else if(hitCollider.tag.Equals("PlayerCollision"))
        {
            Destroy(owner.gameObject);
        }
        base.ActOnCollision(hitCollider);
    }
}
