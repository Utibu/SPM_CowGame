using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : DroppableObject
{
    public AudioClip CoinSound;

    override public void Start()
    {
        base.Start();
        GameManager.instance.SaveManager.Coins.Add(GetComponent<Saveable>().Id, this);
    }

    public override void OnPlayerTriggerEnter(Collider hitCollider)
    {
        base.OnPlayerTriggerEnter(hitCollider);
        GameManager.instance.coinCount++;
        GameManager.instance.totalCoinCount++;
        LevelManager.instance.pickedCoins++;
        EventSystem.Current.FireEvent(new PlaySoundEvent(transform.position, CoinSound, 0.8f, 1.0f, 1.4f));
        // sound event here instead??
    }
}
