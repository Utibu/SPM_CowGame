using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveCooldownTrigger : BaseTrigger
{
    public override void OnPlayerTriggerEnter() {
        GameManager.instance.player.hasFreeDash = true;
        Debug.Log("Gets free dash!");
    }
}
