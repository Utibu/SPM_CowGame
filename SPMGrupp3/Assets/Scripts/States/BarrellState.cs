using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BarrellStateMachine/BarrellState")]
public class BarrellState : PhysicsBaseState
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
        if (hitCollider.tag.Equals("Enemy"))
        {
            Bonde bonde = hitCollider.GetComponent<Bonde>();
            bonde.PlayerDash();
        }
        base.ActOnCollision(hitCollider);
    }
}
