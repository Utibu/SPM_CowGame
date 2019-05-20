using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boss/BossTransitionState")]
public class BossTransitionState : BossBaseState
{

    private Vector3 destination;
    private float rangeFromDestination = 2f;


    public override void Enter()
    {
        destination = owner.Destination;
        
        owner.agnes.enabled = false;
        
        
    }

    public override void Update()
    {
        if(!Helper.IsWithinDistance(owner.transform.position, destination, rangeFromDestination))
        {
            
            owner.transform.position = Vector3.Slerp(owner.transform.position, destination, speed * Time.deltaTime);
            
        }
        else
        {
            
            if(Helper.IsWithinDistance(owner.transform.position, owner.snipeLocation.transform.position, rangeFromDestination) && owner.lastState.GetType() != typeof(BossSnipeState))
            {
                owner.Transition<BossSnipeState>();
            }
            else
            {
                owner.Transition<BossAttackState>();
            }
            
        }
    }

    public override void Leave()
    {
        owner.agnes.enabled = true;
    }
}
