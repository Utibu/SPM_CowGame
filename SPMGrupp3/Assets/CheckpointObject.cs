using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointObject : MonoBehaviour
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
}
