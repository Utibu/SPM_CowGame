using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HayBaleDrop : DroppableObject
{

    public float healthPoints;


    // Start is called before the first frame update
    void Start()
    {
        EventSystem.Current.RegisterListener<OnPlayerDiedEvent>(ResetHay);
    }

    public override void OnPlayerTriggerEnter(Collider hitCollider)
    {
        //base.OnPlayerTriggerEnter(hitCollider);
        Debug.Log("HAYBALE");
        player.health += healthPoints;
        
    }

    private void ResetHay(OnPlayerDiedEvent playerDiedEvent)
    {
        gameObject.SetActive(true);
    }

}
