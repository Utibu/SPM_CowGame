using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : DroppableObject
{
    public AudioClip CoinSound;

    public override void OnEnter()
    {
        base.OnEnter();
        GameManager.instance.coinCount++;
        LevelManager.instance.pickedCoins++;
        EventSystem.Current.FireEvent(new PlaySoundEvent(transform.position, CoinSound, 0.8f, 1.0f, 1.4f));
        // sound event here instead??
    }
}
