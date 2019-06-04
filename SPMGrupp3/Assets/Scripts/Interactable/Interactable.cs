//Main Author: Niklas Almqvist
//Secondary Author: Joakim Ljung, Sofia Kauko
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : Triggable
{
    public float interactionDuration;
    public BasicTimer interactionTimer { get; private set; }
    [HideInInspector] public bool playerIsInteracting = false;
    
    
    public override void Start()
    {
        base.Start();

    }

    public override void Update()
    {
        base.Update();
        
        if (GameManager.instance.inputManager.CancelKeyDown())
        {
            OnCancelInteraction();

            if (playerIsInteracting && interactionTimer.GetElapsedTime() > 0)
            {
                Debug.Log("cancel");
                interactionTimer.Reset();
                GameManager.instance.player.canTakeInput = true;
                playerIsInteracting = false;
                UIManager.instance.HideInteractionMeter();
                GameManager.instance.player.anim.SetTrigger("IsFinishingEatingTrigger");
                GameManager.instance.player.anim.ResetTrigger("IsStartingToEatTrigger");
            }
        }
        
        if(interactionTimer != null && playerIsInteracting && interactionTimer.IsCompleted(Time.deltaTime, true, true))
        {
            Debug.Log("firing interact event");
            //Debug.Log(gameObject.name);
            playerIsInteracting = false;
            OnInteracted();

        }
    }

    public virtual void PlayerInteraction()
    {
        if(playerIsInteracting == false)
        {
            Debug.Log("interacting");
            UIManager.instance.ShowInteractionMeter(interactionDuration);
            playerIsInteracting = true;
            interactionTimer = new BasicTimer(interactionDuration);
            GameManager.instance.player.canTakeInput = false;
            //start eating
            GameManager.instance.player.anim.SetTrigger("IsStartingToEatTrigger");
            GameManager.instance.player.anim.ResetTrigger("IsFinishingEatingTrigger");
        }
    }

    protected virtual void OnCancelInteraction()
    {

    }

    protected virtual void OnInteracted()
    {
        
    }
}
