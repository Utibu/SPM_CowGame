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

        CheckForPlayerCollision();

        time += Time.deltaTime;

        if ((time > 10 && owner.velocity.magnitude < 1f) || time % 60 > 120)
        {
            Destroy(owner.gameObject);
        }
    }

    public override void Leave()
    {
        base.Leave();
    }

    public void CheckForPlayerCollision()
    {
        RaycastHit[] hits = Physics.BoxCastAll(owner.transform.position, owner.objectCollider.bounds.size / 2, owner.velocity.normalized, owner.transform.rotation, float.MaxValue, ((RollingBallStateMachine)owner).playerMask);
        foreach (RaycastHit hit in hits)
        {
            if(hit.collider.tag.Equals("Player"))
            {
                //Debug.Log(hit.distance);
                //if (((owner.transform.position + (owner.velocity * Time.deltaTime)) - (owner.transform.position + (hit.collider.GetComponent<PlayerStateMachine>().velocity * Time.deltaTime))).magnitude >= (owner.velocity * Time.deltaTime).magnitude)
                //Vector3.Distance(hit.collider.transform.position, owner.transform.position)
                if (hit.distance < 1f)
                {
                    //Debug.LogWarning("HEYO BIA");
                    ActOnPlayerCollision(hit.collider);
                }
                
                return;
            }
            //Debug.Log("hit");
            
        }
    }

    public void ActOnPlayerCollision(Collider hitCollider)
    {
        if(hitCollider.tag.Equals("Player"))
        {
            Debug.LogWarning("PLAYERDASH:" + GameManager.instance.player.velocity.magnitude);
        }
            
        if (hitCollider.tag.Equals("Player") && owner.velocity.magnitude > rockVelocityToHurtPlayer)
        {
            
            GameManager.instance.player.playerValues.health -= 25;
            GameManager.instance.player.velocity = Vector3.zero;
            Destroy(owner.gameObject);
        }
    }
}
