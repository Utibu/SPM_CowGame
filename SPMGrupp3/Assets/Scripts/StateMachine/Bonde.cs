using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class Bonde : StateMachine
{

    //bondevariabler
    private BoxCollider boxref;
    public NavMeshAgent agnes;
    public GameObject[] patrolPoints;
    public GameObject weapon;
    //public GameObject player;
    public PlayerStateMachine player;
    public float stunTime;
    public float toAttack;
    public float maxVisibility;

    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        boxref = GetComponent<BoxCollider>();
        agnes = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStateMachine>();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public virtual void PlayerDash()
    {
        Transition<BondeStunState>();
    }
}
