using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BarrellStateMachine/BarrellState")]
public class BarrellState : PhysicsBaseState
{

    public float moveMultiplier;

    private Vector3 originalTransform;

    public override void Enter()
    {
        base.Enter();
        originalTransform = ((BarrellStateMachine)owner).transform.position;
        ((BarrellStateMachine)owner).moveMultiplier = moveMultiplier;
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
        if (hitCollider.tag.Equals("Enemy"))
        {
            Bonde bonde = hitCollider.GetComponent<Bonde>();
            bonde.PlayerDash();
        }

        base.ActOnCollision(hitCollider);
    }

    public override void ActOnTrigger(Collider hitCollider)
    {
        base.ActOnTrigger(hitCollider);

        if (hitCollider.tag.Equals("Killzone"))
        {
            ((BarrellStateMachine)owner).velocity = Vector3.zero;
            ((BarrellStateMachine)owner).transform.position = originalTransform;
        }
    }
}
