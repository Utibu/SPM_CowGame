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
        /*
        if (GameManager.instance.inputManager.EventKeyDown())
        {
            if(playerIsInteracting && interactionTimer.GetElapsedTime() > 0)
            {
                Debug.Log("cancel");
                interactionTimer.Reset();
                GameManager.instance.player.takeInput = true;
                playerIsInteracting = false;
                canInteractTimer = new BasicTimer(1f);
            }
        }
        */
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
        }
    }

    protected virtual void OnInteracted()
    {
        
    }
}
