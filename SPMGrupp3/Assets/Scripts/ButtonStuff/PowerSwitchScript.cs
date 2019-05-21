using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSwitchScript : ButtonScript
{
    public string colorOfControlledLight;
    public GameObject PowerBox;

    public override void OnPlayerTriggerEnter(Collider hitCollider)
    {
        base.OnPlayerTriggerEnter(hitCollider);
        PowerBox.GetComponent<PowerBoxScript>().ToggleLightButton(colorOfControlledLight);
    }
}
