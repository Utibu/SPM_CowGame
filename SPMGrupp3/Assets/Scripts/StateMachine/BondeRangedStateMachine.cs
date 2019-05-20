using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BondeRangedStateMachine : Bonde
{
    public GameObject bullet;
    public GameObject gun;
    public float attackSpeed;
    public float reloadTime;
    public float bulletsBeforeReload;
    [HideInInspector] public float bulletsShotSinceReload;

    public override void PlayerDash(Vector3 velocity)
    {
        base.PlayerDash(velocity);
        
            //Transition<BondeRangedStunState>();
        
    }
    public override void Update()
    {
        base.Update();
        
    }
}
