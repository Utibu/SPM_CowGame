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
    public GameObject itemDrop;
    //public GameObject player;
    public PlayerStateMachine player;
    public int toughness = 1;
    public float stunTime;
    public float toAttack;
    public float maxVisibility;
    public bool customAttackDamage;
    public float attackDamage;


    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        boxref = GetComponent<BoxCollider>();
        agnes = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStateMachine>();
    }

    public void Start()
    {
        
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public virtual void PlayerDash()
    {
        toughness -= 1;
        if(toughness <= 0)
        {
            Debug.Log("DASH");
            Transition<BondeStunState>();
        }
        
    }
}
