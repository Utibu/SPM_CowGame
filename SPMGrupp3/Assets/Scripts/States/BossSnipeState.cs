using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSnipeState : BossBaseState
{
    public override void Enter()
    {
        base.Enter();
        owner.transform.position = owner.snipeLocation.transform.position;
    }

    public override void Update()
    {
        
    }
}
