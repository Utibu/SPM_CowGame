﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BondeRangedStateMachine : Peasant
{
    public GameObject bullet;
    public GameObject gun;
    public float attackSpeed;
    public float reloadTime;
    public float bulletsBeforeReload;
    [HideInInspector] public float bulletsShotSinceReload;
    public AudioClip GunSound;
    public AudioClip ReloadSound;

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
