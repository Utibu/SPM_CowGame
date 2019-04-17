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
        if(health >= 0)
        {
            //healthBar.fillAmount = health / maxHealth;
        }
        
    }

    public void OnHayEatingFinished(HayEatingFinishedEvent eventInfo)
    {
        health += 20;
        Debug.Log(eventInfo.eventDescription);
    }
}
