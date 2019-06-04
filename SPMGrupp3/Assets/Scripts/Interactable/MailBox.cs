//Main Author: Sofia Kauko
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MailBox : Interactable
{

    private string message;
    public Sprite letterImage;
    private bool isShowing = false;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        message = "";
    }

    public override void Update()
    {
        base.Update();
        
        if (isShowing && !TriggerCollider.bounds.Intersects(GameManager.instance.player.objectCollider.bounds))
        {
            OnCancelInteraction();
            isShowing = false;
        }
        
    }

    public override void PlayerInteraction()
    {
        //base.PlayerInteraction();
        Debug.Log("interacting with mailbox");
        UIManager.instance.ShowSmallMessage("You found a letter", message, letterImage);
        isShowing = true;
    }

    protected override void OnCancelInteraction()
    {
        base.OnCancelInteraction();
        UIManager.instance.HideMessages();
    }


}
