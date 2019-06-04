//Main Author: Niklas Almqvist
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ButtonScript : Triggable
{

    public override void OnPlayerTriggerEnter(Collider hitCollider) {
        base.OnPlayerTriggerEnter(hitCollider);
        if(GameManager.instance.inputManager.EventKeyDown() == false)
        {
            return;
        }
    }

}
