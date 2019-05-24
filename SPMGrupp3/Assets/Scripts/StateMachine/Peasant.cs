using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using UnityEngine.UI;

public class Peasant : StateMachine
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
    protected float currentToughness;
    public float stunTime;
    public float toAttack;
    public float maxVisibility;
    public bool customAttackDamage;
    public float attackDamage;

    protected bool isPaused = false;
    public bool isDying = false;
    protected BasicTimer timer;
    protected BasicTimer Gracetimer;
    [SerializeField] protected float graceTime = 1.5f;
    [SerializeField] private float knockBackMultiplier = 1.1f;
    public bool DoingKnockback { get; set; }
    private Vector3 knockbackDirection = Vector3.zero;
    public bool DebugThis = false;
    private bool isLethalHit = false;
    [SerializeField] private float stunLengthToGround;
    public float StunLengthToGround { get { return stunLengthToGround; } private set { stunLengthToGround = value; } }

    public Image healthMeter;


    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        currentToughness = toughness;
        boxref = GetComponent<BoxCollider>();
        agnes = GetComponent<NavMeshAgent>();
        DoingKnockback = false;
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
        UnregisterEnemy();
    }

    public void UnregisterEnemy()
    {
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

        //Debug.Log(agnes.velocity + " CURRENT STATE: " + GetCurrentState().GetType());

        //while timer is tickinh, bonde cant take dmg
        if (Gracetimer != null && Gracetimer.IsCompleted(Time.deltaTime, false, true))
        {
            Gracetimer = null;
        }

        
        if (timer != null && timer.IsCompleted(Time.deltaTime, false, true))
        {
            timer = null;
            if(isLethalHit)
            {
                isDying = true;
                currentToughness = toughness; // lives are reset. 
            } else
            {
                DoingKnockback = false;
                agnes.velocity = Vector3.zero;
            }
            
            return;
        } else
        {
            if(DoingKnockback)
            {
                agnes.velocity -= knockbackDirection * Time.deltaTime;
            }
        }
        base.Update();
    }

    public virtual void PlayerDash(Vector3 velocity)
    {
        if(Gracetimer == null)
        {
            currentToughness -= 1;

            //agnes.velocity += agnes.velocity * -1 * 100f;
            knockbackDirection = velocity.normalized;
            agnes.velocity = knockbackDirection.normalized * (5f + velocity.magnitude);
            //Sets to false in stun leave
            DoingKnockback = true;
            Gracetimer = new BasicTimer(graceTime);
            //Debug.Log("NEW VELOCITY: " + agnes.velocity);
            timer = new BasicTimer(0.5f);

            if (currentToughness <= 0)
            {
                isLethalHit = true;
                EventSystem.Current.FireEvent(new EnemyDieEvent("Bonde died", gameObject));
            }

            if(healthMeter != null)
            {
                healthMeter.fillAmount = currentToughness / toughness;
            }
        }
    }
}
