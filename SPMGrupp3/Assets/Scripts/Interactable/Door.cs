using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Dashable
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public override void OnPlayerCollideEnter(Collider hitCollider, out bool skipCollision, int dashLevel)
    {
        base.OnPlayerCollideEnter(hitCollider, out skipCollision, dashLevel);

        if (dashLevel >= requiredLevel)
        {
            Destroy(gameObject);
            skipCollision = true;
        }
        else
        {
            GameManager.instance.player.velocity *= -1;
        }
        skipCollision = false;

    }
}
