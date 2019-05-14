using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ButtonScript : Triggable
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void OnPlayerTriggerEnter(Collider hitCollider) {
        base.OnPlayerTriggerEnter(hitCollider);
        if(GameManager.instance.inputManager.EventKeyDown() == false)
        {
            return;
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
