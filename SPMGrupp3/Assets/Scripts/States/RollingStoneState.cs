using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PhysicsSateMachine/RollingStoneState")]
public class RollingStoneState : PhysicsBaseState
{
    // Start is called before the first frame update
    public override void Enter()
    {
        base.Enter();
        Debug.Log("enter rolling stioneSSSSSS");
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public override void Leave()
    {
        base.Leave();
    }
}
