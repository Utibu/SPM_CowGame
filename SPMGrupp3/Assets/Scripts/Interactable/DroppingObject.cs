using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppingObject : Dashable
{
    public GameObject drop;

    public override void OnPlayerCollideEnter(Collider hitCollider, out bool skipCollision, int dashLevel)
    {
        base.OnPlayerCollideEnter(hitCollider, out skipCollision, dashLevel);
        skipCollision = false;
        GameObject go = Instantiate(drop.gameObject, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
