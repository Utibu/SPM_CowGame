using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class Bonde : StateMachine
{

    //bondevariabler
    private BoxCollider boxref;
    [HideInInspector] public NavMeshAgent agnes;
    public GameObject[] patrolPoints;
    public GameObject weapon;
    public GameObject itemDrop;
    public LayerMask layermask;
    //public GameObject player;
    public PlayerStateMachine player;
    public float toughness = 1;
    public float stunTime;
    public float toAttack;
    public float maxVisibility;
    public bool customAttackDamage;
    public float attackDamage;

    [SerializeField] protected float graceTime = 2f;
    protected float timeSinceLastHit = 0f;
    protected bool isPaused = false;
    public bool isDying = false;
    protected BasicTimer timer;


    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        boxref = GetComponent<BoxCollider>();
        agnes = GetComponent<NavMeshAgent>();
        
    }

    public override void Start()
    {
        player = GameManager.instance.player;
        EventSystem.Current.RegisterListener<PauseEvent>(Pause);
        EventSystem.Current.RegisterListener<ResumeEvent>(Resume);
        EventSystem.Current.RegisterListener<UnregisterListenerEvent>(UnregisterEvents);

    }

    private void UnregisterEvents(UnregisterListenerEvent eventInfo)
    {
        Debug.Log("UNREGISTER");
        EventSystem.Current.UnregisterListener<PauseEvent>(Pause);
        EventSystem.Current.UnregisterListener<ResumeEvent>(Resume);
    }

    private void Pause(PauseEvent eventInfo)
    {
        isPaused = true;
        if(agnes.isActiveAndEnabled)
        {
            agnes.isStopped = true;
        }
        
    }

    private void Resume(ResumeEvent eventInfo)
    {
        isPaused = false;
        if (agnes.isActiveAndEnabled)
        {
            agnes.isStopped = false;
        }
    }

    // Update is called once per frame
    public override void Update()
    {

        if(isPaused)
        {
            return;
        }

        if (timer != null && timer.IsCompleted(Time.deltaTime, false, true))
        {
            timer = null;
            isDying = true;
            return;
        }

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
            agnes.velocity += player.velocity * 1.8f;
            timeSinceLastHit = 0;

            if (toughness <= 0)
            {
                timer = new BasicTimer(0.5f);
                EventSystem.Current.FireEvent(new EnemyDieEvent("Bonde died", gameObject));
            }
        }
        
        
    }
}
