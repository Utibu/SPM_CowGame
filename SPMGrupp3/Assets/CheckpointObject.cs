using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointObject : Triggable
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        LevelManager.instance.RegisterCheckpoint(this.transform);
    }

    public override void OnPlayerTriggerEnter(Collider hitCollider)
    {
        base.OnPlayerTriggerEnter(hitCollider);
        LevelManager.instance.RegisterCheckpointTaken(hitCollider.transform);
    }
}
