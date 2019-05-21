﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerValues : MonoBehaviour
{

    public float health;
    public float maxHealth;
    public Image healthBar;
    public Text healthText;
    public Image healthBarBackground;
    public bool gotKey1 = false;
    [SerializeField] private float healhBarSizeX;
    [SerializeField] private float healthBarSizeY;

    // Start is called before the first frame update
    void Start()
    {
        EventSystem.Current.RegisterListener<HayEatingFinishedEvent>(OnHayEatingFinished);
    }

    // Update is called once per frame
    void Update()
    {
        

        if(health > 0)
        {
            if(healthBar != null && healthText != null)
            {
                healthBar.fillAmount = health / maxHealth;
                healthBar.rectTransform.sizeDelta = new Vector2(maxHealth + healhBarSizeX, healthBarSizeY);
                healthText.text = health + "/" + maxHealth;
                healthBarBackground.rectTransform.sizeDelta = healthBar.rectTransform.sizeDelta;
            }
            
        } else
        {
            Die();
        }

        if(Input.GetKeyDown(KeyCode.K))
        {
            Die();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            health = maxHealth;
        }
        
    }

    public void Die()
    {
        //UIManager.instance.ShowDeathMessage();
        health = maxHealth;
        if (healthBar != null)
        {
            healthBar.fillAmount = health / maxHealth;
        }
        EventSystem.Current.FireEvent(new OnPlayerDiedEvent(this.gameObject, "Player died"));
    }

    public void OnHayEatingFinished(HayEatingFinishedEvent eventInfo)
    {
        if(health < maxHealth)
        {
            health += 20;
            if(health > maxHealth)
            {
                health = maxHealth;
            }
        }
        
    }
}
