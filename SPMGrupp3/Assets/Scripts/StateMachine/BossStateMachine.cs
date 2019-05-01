using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStateMachine : Bonde
{
    public GameObject bullet;
    public GameObject gun;
    public float attackSpeed;
    public float reloadTime;
    public float bulletsBeforeReload;
    [HideInInspector]
    public float bulletsShotSinceReload;

    public float health = 100;

    public override void PlayerDash()
    {
        Transition<BossStunState>();
    }
}
