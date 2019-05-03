using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "RollingBallStateMachine/RollingBallState")]
public class RollingBallState : PhysicsBaseState
{
    private float time;
    public float rockVelocityToHurtPlayer = 4f;

    public override void Enter()
    {
        base.Enter();
        //((RollingBallStateMachine)owner).moveMultiplier = moveMultiplier;
    }

    public override void Update()
    {
        base.Update();
        time += Time.deltaTime;

        if ((time % 60 > 10 && owner.velocity.magnitude < 1f) || time % 60 > 120)
        {
            Destroy(owner.gameObject);
        }
    }

    public override void Leave()
    {
        base.Leave();
    }

    public override void ActOnCollision(Collider hitCollider)
    {
        if(hitCollider.tag.Equals("Player"))
        {
            Debug.LogWarning("PLAYERDASH:" + GameManager.instance.player.velocity.magnitude);
        }
            
        if (hitCollider.tag.Equals("Player") && owner.velocity.magnitude > rockVelocityToHurtPlayer)
        {
            
            GameManager.instance.player.playerValues.health -= 25;
            Destroy(owner.gameObject);
        }
    }
}
