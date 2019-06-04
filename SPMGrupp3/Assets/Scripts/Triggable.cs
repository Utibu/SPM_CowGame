//Main Author: Niklas Almqvist
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Triggable : MonoBehaviour
{

    protected Collider TriggerCollider;

    private void Awake()
    {
        
    }

    public virtual void Start()
    {
        TriggerCollider = GetComponent<Collider>();
    }

    public virtual void OnPlayerTriggerEnter(Collider hitCollider)
    {

    }

    public virtual void Update()
    {
    }
}
