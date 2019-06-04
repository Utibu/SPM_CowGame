//Main Author: Niklas Almqvist
//Secondary Author: Joakim Ljung, Sofia Kauko
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HayInteractable : Interactable
{

    public float healthReplenished;

    public override void Start()
    {
        base.Start();
        EventSystem.Current.RegisterListener<OnPlayerDiedEvent>(ResetHay);
        EventSystem.Current.RegisterListener<UnregisterListenerEvent>(Unregister);
    }

    public override void Update()
    {
        base.Update();
        

    }

    protected override void OnInteracted()
    {
        base.OnInteracted();

        EventSystem.Current.FireEvent(new HayEatingFinishedEvent(gameObject, healthReplenished, "Hay eaten"));
        GameManager.instance.player.anim.SetTrigger("IsFinishingEatingTrigger");
        GameManager.instance.player.anim.ResetTrigger("IsStartingToEatTrigger");
        //Debug.Log("interacted");
        //transform.parent.gameObject.SetActive(false);
    }

    private void ResetHay(OnPlayerDiedEvent playerDiedEvent)
    {
        gameObject.transform.parent.gameObject.SetActive(true);
    }

    private void Unregister(UnregisterListenerEvent eventInfo)
    {
        EventSystem.Current.UnregisterListener<OnPlayerDiedEvent>(ResetHay);
    }
}
