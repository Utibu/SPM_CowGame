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

    protected bool isPaused = false;
    public bool isDying = false;
    protected BasicTimer timer;
    private BasicTimer Gracetimer;
    [SerializeField] private float knockBackMultiplier = 1.1f;


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
        if (Gracetimer != null && Gracetimer.IsCompleted(Time.deltaTime, false, true))
        {
            Gracetimer = null;
        }
        if (timer != null && timer.IsCompleted(Time.deltaTime, false, true))
        {
            timer = null;
            isDying = true;
            return;
        }
        base.Update();
    }

    public virtual void PlayerDash()
    {
        
        if(Gracetimer == null)
        {
            toughness -= 1;
            agnes.velocity += player.velocity * knockBackMultiplier;
            Gracetimer = new BasicTimer(1.5f);

            if (toughness <= 0)
            {
                isDying = true;
                timer = new BasicTimer(0.5f);
                EventSystem.Current.FireEvent(new EnemyDieEvent("Bonde died", gameObject));
            }
        }
        
        
    }
}
