using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HayBaleDrop : DroppableObject
{
        override public void Start()
    {
        GameManager.instance.SaveManager.Haybales.Add(GetComponent<Saveable>().Id, gameObject);
    }
}
