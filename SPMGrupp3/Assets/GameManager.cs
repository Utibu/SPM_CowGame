﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    [HideInInspector] public Transform currentCheckpoint;
    public int deathCount = 0;
    public PlayerStateMachine player;
    public Transform originalSpawnTransform;
    public Text velocityText;
    public Image dashCooldownImage;
    [SerializeField] private int coinsToHPIncrease = 5;
    [HideInInspector] public int coinCount = 0;
    public bool debug;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        //QualitySettings.vSyncCount = 0;  // VSync must be disabled
        //Application.targetFrameRate = 45;

    }
    // Start is called before the first frame update
    void Start()
    {
        

        EventSystem.Current.RegisterListener<OnPlayerDiedEvent>(Respawn);
        if(!debug)
        {
            player.transform.position = originalSpawnTransform.position;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(velocityText != null)
        {
            velocityText.text = "Velocity: " + player.velocity.magnitude;
        }

        if(dashCooldownImage != null)
        {
            if(player.allowedToDash)
            {
                dashCooldownImage.fillAmount = 1;
            } else
            {
                dashCooldownImage.fillAmount = (player.elapsedDashTime % 60) / player.dashCooldown;
            }
            
        }

        if(coinCount >= coinsToHPIncrease)
        {
            player.playerValues.maxHealth += 20;
            player.playerValues.health = player.playerValues.maxHealth;
            coinCount = 0;
        }
    }

    public void CheckpointTaken(Transform checkPointTransform)
    {
        currentCheckpoint = checkPointTransform.Find("SpawnPoint");
        checkPointTransform.gameObject.SetActive(false);
    }

    void Respawn(OnPlayerDiedEvent eventInfo)
    {
        deathCount++;
        if(deathCount <= 3 && currentCheckpoint != null)
        {
            player.Respawn(currentCheckpoint.transform.position);
        } else
        {
            player.Respawn(originalSpawnTransform.position);
        }
    }

    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }
}
