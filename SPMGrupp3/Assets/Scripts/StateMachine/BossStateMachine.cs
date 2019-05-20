﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossStateMachine : Bonde
{
    public GameObject bullet;
    public GameObject gun;
    public float attackSpeed;
    public float reloadTime;
    public float bulletsBeforeReload;
    [HideInInspector]
    public float bulletsShotSinceReload;
    public AudioClip GunSound;

    public float health = 100f;
    public float damageOnDash = 25f;

    public GameObject underlingPrefab;
    public GameObject underlingSpawnArea;
    [SerializeField] private float spawnRadius;
    public GameObject snipeLocation;
    public Vector3 Destination;

    private float timeSinceLastHit = 0f;
    //public float GraceTime = 1f;
    public float timeBetweenSpawns = 0.2f;
    public int underlingQuantityPerWave = 4;
    [HideInInspector] public int count = 0;
    //private float currentToughness;
    private List<GameObject> underlingList = new List<GameObject>();
    //Gör private
    public Vector3 originalPosition;

    public Image healthBar;

    protected override void Awake()
    {
        base.Awake();
        currentToughness = toughness;
    }

    public override void Start()
    {
        base.Start();
        originalPosition = transform.position;
    }

    public override void Update()
    {
        base.Update();
    }

    public override void PlayerDash(Vector3 velocity)
    {
        //base.PlayerDash(velocity);
        if (Gracetimer == null)
        {
            currentToughness -= 1;
            transform.position += transform.forward * -2;
            timeSinceLastHit = 0;
            count = 0;
            SpawnUnderling();
            Destination = snipeLocation.transform.position;
            Transition<BossTransitionState>();
        }
        if (currentToughness <= 0)
        {
            GameManager.instance.LoadMenu();
        }
        healthBar.fillAmount =  currentToughness / toughness;
        Debug.Log("Current: " + currentToughness + " Total: " + toughness + " " + currentToughness/toughness);


    }

    public void SpawnUnderling()
    {
        count++;
        GameObject underling = Instantiate(underlingPrefab, (underlingSpawnArea.transform.position), Quaternion.identity);
        Bonde bonde = underling.GetComponent<Bonde>();
        bonde.patrolPoints = patrolPoints;
        bonde.maxVisibility = 25f;
        underlingList.Add(underling);
        if (count < underlingQuantityPerWave)
        {
            Invoke("SpawnUnderling", timeBetweenSpawns);
        }

    }

    private void OnUnderlingDeath(EnemyDieEvent enemyDeath)
    {
        Debug.Log("enemy died");
        if (underlingList.Contains(enemyDeath.enemy))
        {
            Debug.Log("underling died");
            underlingList.Remove(enemyDeath.enemy);

            if (underlingList.Count == 0)
            {
                Destination = originalPosition;
                Transition<BossTransitionState>();
                
            }
        }
    }

}
