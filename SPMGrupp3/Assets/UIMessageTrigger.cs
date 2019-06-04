//Main Author: Niklas Almqvist
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

    
    public bool IsShowing { get; private set; }

    public void ShowMessage()
    {
        if (rightDescription.Length > 0 && rightSprite != null)
        {
            UIManager.instance.ShowBigMessage(title, description, sprite, rightDescription, rightSprite);
        }
        else
        {
            UIManager.instance.ShowSmallMessage(title, description, sprite);
        }

        IsShowing = true;
        if (shouldPause)
        {
            EventSystem.Current.FireEvent(new PauseEvent(""));
        }
    }

    public override void OnPlayerTriggerEnter(Collider hitCollider)
    {
        base.OnPlayerTriggerEnter(hitCollider);
        ShowMessage();
        
    }

    public override void Start()
    {
        base.Start();
        IsShowing = false;
    }

    public override void Update()
    {
        base.Update();
        if(IsShowing && GameManager.instance.inputManager.CancelKeyDown())
        {
            EventSystem.Current.FireEvent(new ResumeEvent(""));
            UIManager.instance.HideMessages();
            if(TriggerCollider != null)
            {
                TriggerCollider.gameObject.SetActive(false);
            }
            
        }
    }

    //TODO: Removed because it's not LastingTriggable anymore
    /*protected override void OnPlayerTriggerLeave()
    {
        base.OnPlayerTriggerLeave();
        UIManager.instance.HideMessage();
    }*/
}
