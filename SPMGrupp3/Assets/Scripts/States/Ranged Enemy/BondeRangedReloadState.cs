using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BondeRanged/BondeRangedReloadState")]
public class BondeRangedReloadState : BondeRangedBaseState
{

    private float time;

    public override void Enter()
    {
        base.Enter();
        time = 0f;
        owner.agnes.isStopped = true;
        EventSystem.Current.FireEvent(new PlaySoundEvent(owner.transform.position, owner.ReloadSound, 1f, 0.9f, 1.1f));

    }

    public override void Update()
    {
        base.Update();
        if(time > owner.reloadTime && owner.DoingKnockback == false)
        {
            owner.Transition<BondeRangedAttackState>();
        } else
        {
            time += Time.deltaTime;
        }
     
    }

    public override void Leave()
    {
        base.Leave();
        owner.agnes.isStopped = false;

    }

}
