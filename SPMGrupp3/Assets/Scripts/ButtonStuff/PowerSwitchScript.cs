using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSwitchScript : ButtonScript
{
    public string colorOfControlledLight;
    public GameObject PowerBox;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    public override void OnPlayerTriggerEnter(Collider hitCollider)
    {
        base.OnPlayerTriggerEnter(hitCollider);
        PowerBox.GetComponent<PowerBoxScript>().ToggleLightButton(colorOfControlledLight);
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }
}
