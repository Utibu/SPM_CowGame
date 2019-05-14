using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointObject : Triggable
{
    // Start is called before the first frame update
    void Start()
    {
        LevelManager.instance.RegisterCheckpoint(this.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnPlayerTriggerEnter(Collider hitCollider)
    {
        base.OnPlayerTriggerEnter(hitCollider);
        LevelManager.instance.RegisterCheckpointTaken(hitCollider.transform);
    }
}
