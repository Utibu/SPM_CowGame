using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HayBaleDrop : DroppableObject
{
    // Start is called before the first frame update
    public float healthPoints;
    void Start()
    {
        
    }

    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("HAYBALE");
        player.health += healthPoints;
    }

}
