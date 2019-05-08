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
    [HideInInspector] public float countdown;
    public float cooldown = 1.2f;

    public float graceTime = 2f;
    private float timeSinceLastHit = 0f;


    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        boxref = GetComponent<BoxCollider>();
        agnes = GetComponent<NavMeshAgent>();
        
    }

    public void Start()
    {
        player = GameManager.instance.player;
        countdown = cooldown;
    }

    // Update is called once per frame
    public override void Update()
    {
        if(timeSinceLastHit < 10.0f)
        {
            timeSinceLastHit += Time.deltaTime;
        }
        
        base.Update();
    }

    public virtual void PlayerDash()
    {
        
        if(timeSinceLastHit > graceTime)
        {
            toughness -= 1;
            timeSinceLastHit = 0;
        }
        if(toughness <= 0)
        {
            Debug.Log("DASHED");
            Transition<BondeStunState>();
        }
        
    }
}
