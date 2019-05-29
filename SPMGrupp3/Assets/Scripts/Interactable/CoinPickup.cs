using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : DroppableObject
{
    [SerializeField] private AudioClip[] CoinSounds;

    override public void Start()
    {
        base.Start();
        GameManager.instance.SaveManager.Coins.Add(GetComponent<Saveable>().Id, this);
    }

    public override void OnPlayerTriggerEnter(Collider hitCollider)
    {
        base.OnPlayerTriggerEnter(hitCollider);
        Debug.Log("PLAYING!");
        GameManager.instance.coinCount++;
        GameManager.instance.totalCoinCount++;
        LevelManager.instance.pickedCoins++;
        EventSystem.Current.FireEvent(new PlaySoundEvent(transform.position, GetCoinClip(), 0.5f, 0.95f, 1.05f));
        
        // sound event here instead??
    }

    private AudioClip GetCoinClip()
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
