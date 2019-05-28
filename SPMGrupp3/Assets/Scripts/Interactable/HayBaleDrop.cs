using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HayBaleDrop : DroppableObject
{
    
    public float healthPoints;


    // Start is called before the first frame update
    override public void Start()
    {
        EventSystem.Current.RegisterListener<OnPlayerDiedEvent>(ResetHay);
        EventSystem.Current.RegisterListener<UnregisterListenerEvent>(Unregister);
        GameManager.instance.SaveManager.Haybales.Add(GetComponent<Saveable>().Id, gameObject);
    }

    public override void OnPlayerTriggerEnter(Collider hitCollider)
    {
        Debug.Log("haybaledrop");
        //base.OnPlayerTriggerEnter(hitCollider);
        Debug.Log("HAYBALE");
        player.health += healthPoints;
        
    }

    private void ResetHay(OnPlayerDiedEvent playerDiedEvent)
    {
        gameObject.SetActive(true);
    }

    private void Unregister(UnregisterListenerEvent eventInfo)
    {
        EventSystem.Current.UnregisterListener<OnPlayerDiedEvent>(ResetHay);
    }
    
}
