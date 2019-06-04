//Main Author: Joakim Ljung
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerStateMachine/InteractState")]
public class InteractState : PlayerBaseState
{
    //Gör så att den här klassen tar emot ett oninteractevent. Om man inte avbryter den så skickas ett hayeatingevent. 
    //Kanske kolla vad för typ av objekt man interactar med.

    HayInteractable hayInteractable;

    public override void Enter()
    {
        base.Enter();
        //EventSystem.Current.RegisterListener<OnInteractionFinishedEvent>(OnInteractionFinished);
    }
    
    public override void Update()
    {
        base.Update();
        if (GameManager.instance.inputManager.EventKeyDown())
        {
            owner.Transition<WalkState>();
        }

        /*
        if (GameManager.instance.inputManager.EventKeyDown())
        {
            //interactionTimer.Cancel();
            owner.Transition<WalkState>();
        }

        if(interactionTimer.IsCompleted(Time.deltaTime, true, true))
        {
            
            owner.Transition<WalkState>();
        }
        */
    }

    public override void Leave()
    {
        base.Leave();
        //EventSystem.Current.UnregisterListener<OnInteractionFinishedEvent>(OnInteractionFinished);
        

    }
    /*
    private void OnInteractionFinished(OnInteractionFinishedEvent eventInfo)
    {
        HayInteractable hayInteractable = eventInfo.gameObject.GetComponent<HayInteractable>();
        if (hayInteractable != null)
        {
            Debug.Log("firing hay event");
            Debug.Log(eventInfo.gameObject.name);
            EventSystem.Current.FireEvent(new HayEatingFinishedEvent(eventInfo.gameObject, hayInteractable.healthReplenished, "Hay eating finished"));
        }
        owner.Transition<WalkState>();
        Debug.Log("leaving walkstate");
    }
    */
}
