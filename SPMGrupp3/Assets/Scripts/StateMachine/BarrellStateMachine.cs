using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrellStateMachine : PhysicsStateMachine
{

    [HideInInspector] public float moveMultiplier;
    public bool movable;
    [SerializeField] private AudioClip[] interactionSounds;

    override public void Start()
    {
        GameManager.instance.SaveManager.MovableObjects.Add(GetComponent<Saveable>().Id, gameObject);
    }

    public void Move(Vector3 playerVelocity)
    {
        if(!movable)
        {
            return;
        }

        if(moveMultiplier <= 0)
        {
            moveMultiplier = 1;
        }

        playerVelocity = new Vector3(playerVelocity.x, 0f, playerVelocity.z);

        velocity += playerVelocity.normalized * playerVelocity.magnitude * moveMultiplier;
    }

    public AudioClip GetClip()
    {
        if (interactionSounds.Length == 0)
        {
            return null;
        }
        else
        {
            return interactionSounds[Random.Range(0, interactionSounds.Length)];
        }
    }

}
