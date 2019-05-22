using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateScript : Collidable
{

    public GameObject gateKey;


    public override void OnPlayerCollideEnter(Collider hitCollider, out bool skipCollision)
    {

        if (LevelManager.instance.hasGateKey)
        {
            skipCollision = true;
            Open();
        }
        else
        {
            skipCollision = false;
            if (GameManager.instance.player.GetCurrentState().GetType() == typeof(DashState))
            {
                GameManager.instance.player.velocity *= -1;
                GameManager.instance.player.ShakeCamera();
            }
        }
    }

    private void Open()
    {
        // do a nice gate open rotation or start anim or whatever
        
        Destroy(this.gameObject);

    }

    
}
