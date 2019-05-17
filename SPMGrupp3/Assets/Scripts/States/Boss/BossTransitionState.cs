using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boss/BossTransitionState")]
public class BossTransitionState : BossBaseState
{

    private Vector3 destination;
    private float rangeFromDestination = 3f;


    public override void Enter()
    {
        destination = owner.Destination;
        
        owner.agnes.enabled = false;
        
        Debug.Log("Destination: " + destination + " Snipe location: " + owner.snipeLocation.transform.position + " Start position " + owner.originalPosition);
        
    }

    public override void Update()
    {
        if(!Helper.IsWithinDistance(owner.transform.position, destination, rangeFromDestination))
        {
            
            owner.transform.position = Vector3.Slerp(owner.transform.position, destination, speed * Time.deltaTime);
            
        }
        else
        {
            Debug.Log("destination reached");
            
            if(Helper.IsWithinDistance(owner.transform.position, owner.snipeLocation.transform.position, rangeFromDestination) && owner.lastState.GetType() != typeof(BossSnipeState))
            {
                Debug.Log("transitioned to snipe state");
                owner.Transition<BossSnipeState>();
            }
            else
            {
                Debug.Log("transitioned to attack state");
                owner.Transition<BossAttackState>();
            }
            
        }
    }

    public override void Leave()
    {
        owner.agnes.enabled = true;
    }
}
