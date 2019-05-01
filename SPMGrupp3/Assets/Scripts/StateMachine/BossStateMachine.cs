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

    public float health = 100f;
    public float damageOnDash = 25f;

    public GameObject underlingPrefab;
    public GameObject underlingSpawnpoint;

    public float timeBetweenSpawns = 0.2f;
    public int underlingQuantityPerWave = 4;
    private int count = 0;

    private bool graceTime = false;

    public override void PlayerDash()
    {
        if (graceTime)
            return;

        health -= damageOnDash;
        if(health <= 0)
        {
            Transition<BossStunState>();
        }
        count = 0;
        SpawnUnderling();
        graceTime = true;
    }

    public void SpawnUnderling()
    {
        count++;
        underlingPrefab.SetActive(false);
        GameObject underling = Instantiate(underlingPrefab, underlingSpawnpoint.transform.position, Quaternion.identity);
        underling.GetComponent<Bonde>().patrolPoints = this.patrolPoints;
        underling.SetActive(true);
        if (count < underlingQuantityPerWave)
        {
            Invoke("SpawnUnderling", timeBetweenSpawns);
        } else
        {
            graceTime = false;
        }
    }
}
