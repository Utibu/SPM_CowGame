using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triggable : MonoBehaviour
{

    protected Collider TriggerCollider;

    private void Awake()
    {
        
    }

    private void Start()
    {
        CustomStart();
    }

    public virtual void OnPlayerTriggerEnter(Collider hitCollider)
    {

    }

    private void Update()
    {
        CustomUpdate();
    }

    protected virtual void CustomStart()
    {
        TriggerCollider = GetComponent<Collider>();
    }

    public virtual void CustomUpdate()
    {

    }
}
