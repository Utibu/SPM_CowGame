using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boss/BossReloadState")]
public class BossReloadState : BossBaseState
{
    [SerializeField] private float reloadTime;
    private BasicTimer timer;

    public override void Enter()
    {
        timer = new BasicTimer(reloadTime);
        owner.renderColor.material.color = Color.blue;
        owner.agnes.isStopped = true;
        EventSystem.Current.FireEvent(new PlaySoundEvent(owner.transform.position, owner.ReloadSound, 1f, 1f, 1f)); //doesn't work :(
    }

    public override void Update()
    {
        timer.Update(Time.deltaTime);
        
        if (timer.IsCompleted(Time.deltaTime, false, false))
        {
            owner.Transition<BossPatrolState>();
        }
        
    }

    public override void Leave()
    {
        base.Leave();
        owner.renderColor.material.color = Color.white;
        owner.agnes.isStopped = false;
    }


}
