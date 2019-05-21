using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HayInteractable : Interactable
{
    protected override void OnInteracted()
    {
        base.OnInteracted();
        
        EventSystem.Current.FireEvent(new HayEatingFinishedEvent(this.gameObject, "Eating hay finished!"));
        transform.parent.gameObject.SetActive(false);
    }
}
