using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float interactionFreezeTime;
    public bool playerIsInteracting = false;
    public virtual void InvokeInteraction()
    {
        playerIsInteracting = true;
        Invoke("OnInteracted", interactionFreezeTime);
    }

    protected virtual void OnInteracted()
    {
        
    }
}
