using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MailBox : Interactable
{

    private string message;
    public Sprite letterImage;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        message = "Kommer lägga in bild på brev här snart  :)) ";
    }

    public override void Update()
    {
        base.Update();
    }

    public override void PlayerInteraction()
    {
        //base.PlayerInteraction();
        Debug.Log("interacting with mailbox");
        UIManager.instance.ShowSmallMessage("You found a letter", message, letterImage);
    }

    protected override void OnCancelInteraction()
    {
        base.OnCancelInteraction();
        UIManager.instance.HideMessages();
    }


}
