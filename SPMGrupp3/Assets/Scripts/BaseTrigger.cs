﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTrigger : Triggable
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnPlayerTriggerEnter(Collider hitCollider)
    {
        base.OnPlayerTriggerEnter(hitCollider);
    }
}
