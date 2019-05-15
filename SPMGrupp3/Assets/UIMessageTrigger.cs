using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMessageTrigger : Triggable
{
    [SerializeField] private string title;
    [SerializeField] private bool shouldPause;

    [SerializeField] private string description;
    [SerializeField] private Sprite sprite;
    [Header("If these are filled in the message becomes 2 side by side")]
    [SerializeField] private string rightDescription;
    [SerializeField] private Sprite rightSprite;

    
    private bool isShowing = false;

    protected override void CustomStart()
    {
        base.CustomStart();
    }

    public override void OnPlayerTriggerEnter(Collider hitCollider)
    {
        base.OnPlayerTriggerEnter(hitCollider);
        if(rightDescription.Length > 0 && rightSprite != null)
        {
            UIManager.instance.ShowBigMessage(title, description, sprite, rightDescription, rightSprite);
        } else
        {
            UIManager.instance.ShowSmallMessage(title, description, sprite);
        }
        
        isShowing = true;
        if(shouldPause)
        {
            EventSystem.Current.FireEvent(new PauseEvent(""));
        }
        
    }

    public override void CustomUpdate()
    {
        base.CustomUpdate();
        if(isShowing && GameManager.instance.inputManager.ContinueKeyDown())
        {
            EventSystem.Current.FireEvent(new ResumeEvent(""));
            UIManager.instance.HideMessages();
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
