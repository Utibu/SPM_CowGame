using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

[CreateAssetMenu(menuName = "Miniboss/MinibossStunState")]
public class MinibossStunState : BondeStunState
{
    
    public override void Enter()
    {
        base.Enter();
        // drop key around boss
        Instantiate(owner.itemDrop);
        owner.itemDrop.transform.position = owner.transform.position + new Vector3(2.0f,0.0f,2.0f);
        
    }
    
}
