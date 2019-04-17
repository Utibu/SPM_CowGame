using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerStateMachine/InteractState")]
public class InteractState : PlayerBaseState
{

    public override void Enter()
    {
        owner.gameObject.GetComponent<MeshRenderer>().material.color = Color.magenta;
        base.Enter();
    }

    public override void Leave()
    {
        owner.gameObject.GetComponent<MeshRenderer>().material.color = Color.white;
        base.Leave();
    }

    public override void Update()
    {
        base.Update();
    }
}
