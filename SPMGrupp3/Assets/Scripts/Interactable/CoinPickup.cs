//Main Author: Joakim Ljung
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : DroppableObject
{
    [SerializeField] private AudioClip[] CoinSounds;
    protected int coinAmount = 1;


    override public void Start()
    {
        base.Start();
        GameManager.instance.SaveManager.Coins.Add(GetComponent<Saveable>().Id, this);
    }

    public override void OnPlayerTriggerEnter(Collider hitCollider)
    {
        base.OnPlayerTriggerEnter(hitCollider);
        GameManager.instance.coinCount += coinAmount;
        GameManager.instance.totalCoinCount += coinAmount;
        LevelManager.instance.pickedCoins += coinAmount;
        if (GameManager.instance.coinCount < 20)
        {
            EventSystem.Current.FireEvent(new PlaySoundEvent(transform.position, GetCoinClip(), 0.5f, 0.95f, 1.05f));
        }
    }

    protected AudioClip GetCoinClip()
    {
        if (CoinSounds.Length == 0)
        {
            return null;
        }
        else
        {
            return CoinSounds[Random.Range(0, CoinSounds.Length)];
        }
    }
}
