using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMessageTrigger : LastingTriggable
{
    [SerializeField] private string textToShow;

    public override void OnPlayerTriggerEnter(Collider hitCollider)
    {
        base.OnPlayerTriggerEnter(hitCollider);
        UIManager.instance.ShowMessage(textToShow);
    }

    protected override void OnPlayerTriggerLeave()
    {
        base.OnPlayerTriggerLeave();
        UIManager.instance.HideMessage();
    }
}
