using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : Triggable
{
    private Collider keyCollider;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        keyCollider = GetComponent<CapsuleCollider>();
    }

    public override void Update()
    {
        base.Update();
        //transform.RotateAroundLocal(Vector3.up, 0.2f);
        transform.Rotate(Vector3.up, 0.2f);
    }

    public override void OnPlayerTriggerEnter(Collider hitCollider)
    {
        Debug.Log("triggered key");
        // ge key = true till level manager? eller playerValues? sen: destroy
        LevelManager.instance.hasGateKey = true;
        Destroy(gameObject);
    }

    
}
