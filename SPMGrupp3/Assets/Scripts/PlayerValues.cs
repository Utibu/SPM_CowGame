//Main Author: Niklas Almqvist
//Secondary Author: Joakim Ljung, Sofia Kauko
using System.Collections;
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

    bool dieHasBeenCalled = false;

    // Start is called before the first frame update
    void Start()
    {
        EventSystem.Current.RegisterListener<HayEatingFinishedEvent>(OnHayEatingFinished);
    }

    // Update is called once per frame
    void Update()
    {

        if (dieHasBeenCalled)
        {
            return;
        }
        if(health > 0)
        {
            if(healthBar != null && healthText != null)
            {
                healthBar.fillAmount = health / maxHealth;
                healthBar.rectTransform.sizeDelta = new Vector2(maxHealth + healhBarSizeX, healthBarSizeY);
                healthText.text = health + "/" + maxHealth;
                healthBarBackground.rectTransform.sizeDelta = healthBar.rectTransform.sizeDelta;
            }
            
        } else if(dieHasBeenCalled == false)
        {
            
            Die();
        }
        /*
        if(Input.GetKeyDown(KeyCode.K))
        {
            Die();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            health = maxHealth;
        }*/
        
    }

    public void Die()
    {
        dieHasBeenCalled = true;
        StartCoroutine(ExecuteDeathAfterTime(0.5f));
    }

    IEnumerator ExecuteDeathAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        // Code to execute after the delay
        health = maxHealth;
        if (healthBar != null)
        {
            healthBar.fillAmount = health / maxHealth;
        }
        EventSystem.Current.FireEvent(new OnPlayerDiedEvent(this.gameObject, "Player died"));
        dieHasBeenCalled = false;
    }

    public void OnHayEatingFinished(HayEatingFinishedEvent eventInfo)
    {
        GameManager.instance.player.canTakeInput = true;
        if(health < maxHealth)
        {
            health += eventInfo.healthReplenished;
            if(health > maxHealth)
            {
                health = maxHealth;
            }
        }

        eventInfo.gameObject.transform.parent.gameObject.SetActive(false);
        
    }
}
