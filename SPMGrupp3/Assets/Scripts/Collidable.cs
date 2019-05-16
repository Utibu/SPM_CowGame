using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Collidable : MonoBehaviour
{
    

    virtual public void OnPlayerCollideEnter(Collider hitCollider, out bool skipCollision)
    {
        skipCollision = false;
    }
}
