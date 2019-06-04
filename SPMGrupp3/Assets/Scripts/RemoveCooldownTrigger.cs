//Main Author: Niklas Almqvist
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveCooldownTrigger : BaseTrigger
{
    public override void OnPlayerTriggerEnter(Collider hitCollider) {
        base.OnPlayerTriggerEnter(hitCollider);
        GameManager.instance.player.hasFreeDash = true;
        //Debug.Log("Gets free dash!");
    }
}
