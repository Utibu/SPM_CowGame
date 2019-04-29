using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerValues : MonoBehaviour
{

    public float health;
    public float maxHealth;
    public int dashLevel;
    public Image healthBar;

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
            if(healthBar != null)
            {
                healthBar.fillAmount = health / maxHealth;
            }
            
        } else
        {
            Die();
        }

        if(Input.GetKeyDown(KeyCode.K))
        {
            Die();
        }
        
    }

    public void Die()
    {
        health = 100;
        if (healthBar != null)
        {
            healthBar.fillAmount = health / maxHealth;
        }
        EventSystem.Current.FireEvent(new OnPlayerDiedEvent(this.gameObject, "Player died"));
    }

    public void OnHayEatingFinished(HayEatingFinishedEvent eventInfo)
    {
        health += 20;
        Debug.Log(eventInfo.eventDescription);
    }
}
