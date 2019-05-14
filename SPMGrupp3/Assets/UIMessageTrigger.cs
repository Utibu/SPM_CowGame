using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMessageTrigger : Triggable
{
    [SerializeField] private string textToShow;
    [SerializeField] private bool shouldPause;
    private bool isShowing = false;

    protected override void CustomStart()
    {
        base.CustomStart();
        Debug.Log("F");
    }

    public override void OnPlayerTriggerEnter(Collider hitCollider)
    {
        base.OnPlayerTriggerEnter(hitCollider);
        UIManager.instance.ShowMessage(textToShow);
        isShowing = true;
        EventSystem.Current.FireEvent(new PauseEvent(""));
    }

    public override void CustomUpdate()
    {
        base.CustomUpdate();
        if(isShowing && GameManager.instance.inputManager.ContinueKeyDown())
        {
            EventSystem.Current.FireEvent(new ResumeEvent(""));
            UIManager.instance.HideMessage();
            TriggerCollider.gameObject.SetActive(false);
        }
    }

    //TODO: Removed because it's not LastingTriggable anymore
    /*protected override void OnPlayerTriggerLeave()
    {
        base.OnPlayerTriggerLeave();
        UIManager.instance.HideMessage();
    }*/
}
