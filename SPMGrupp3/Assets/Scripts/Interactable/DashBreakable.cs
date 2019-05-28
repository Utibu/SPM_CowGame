using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashBreakable : Dashable
{
    public override void Start()
    {
        base.Start();
    }

    void Update()
    {
        
    }

    public override void OnPlayerCollideEnter(Collider hitCollider, out bool skipCollision, int dashLevel)
    {
        base.OnPlayerCollideEnter(hitCollider, out skipCollision, dashLevel);
        GameManager.instance.player.ShakeCamera();

        if (dashLevel >= requiredLevel)
        {
            skipCollision = true;
            gameObject.SetActive(false);
        }
        else
        {
            GameManager.instance.player.velocity *= -1;
            skipCollision = false;
        }

    }
}
