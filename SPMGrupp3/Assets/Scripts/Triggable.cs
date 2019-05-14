using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triggable : MonoBehaviour
{
    public virtual void OnPlayerTriggerEnter(Collider hitCollider)
    {

    }

    private void Update()
    {
        CustomUpdate();
    }

    public virtual void CustomUpdate()
    {

    }
}
