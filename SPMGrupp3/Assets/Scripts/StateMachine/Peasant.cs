using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using UnityEngine.UI;

public class Peasant : StateMachine
{
    //bondevariabler
    private BoxCollider boxref;
    [SerializeField] private AudioClip[] deathScreams;
    [SerializeField] private AudioClip hitSound;
    [HideInInspector] public NavMeshAgent agnes;
    public GameObject[] patrolPoints;
    public GameObject weapon;
    public GameObject itemDrop;
    public LayerMask layermask;
    public PlayerStateMachine player;
    public float toughness = 1;
    [HideInInspector] public float CurrentToughness;
    
    public float stunTime;
    public float toAttack;
    public float maxVisibility;
    
    // ta bort?
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
    [SerializeField] private float stunLengthToGround;
    public float StunLengthToGround { get { return stunLengthToGround; } private set { stunLengthToGround = value; } }
    public bool IsStunned { get { return GetCurrentState().GetType() == typeof(BondeStunState) || GetCurrentState().GetType() == typeof(BondeRangedStunState) || GetCurrentState().GetType() == typeof(BossStunState) || GetCurrentState().GetType() == typeof(MinibossStunState); } }

    public Image healthMeter;
    [SerializeField] private Image healthMeterBackground;
    public bool ShouldGoAlive = false;


    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        CurrentToughness = toughness;
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
        GameManager.instance.SaveManager.Enemies.Add(GetComponent<Saveable>().Id, this);
    }

    private void UnregisterEvents(UnregisterListenerEvent eventInfo)
    {
        UnregisterEnemy();
    }

    public virtual void UnregisterEnemy()
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

        //while timer is tickinh, peasant cant take dmg
        if (Gracetimer != null && Gracetimer.IsCompleted(Time.deltaTime, false, true))
        {
            Gracetimer = null;
        }

        
        if (timer != null && timer.IsCompleted(Time.deltaTime, false, true))
        {
            timer = null;
            if(CurrentToughness <= 0f)
            {
                isDying = true;
                CurrentToughness = toughness; // lives are reset. 
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
            CurrentToughness -= 1;

            //agnes.velocity += agnes.velocity * -1 * 100f;
            knockbackDirection = velocity.normalized;
            agnes.velocity = knockbackDirection.normalized * (5f + velocity.magnitude);
            //Sets to false in stun leave
            DoingKnockback = true;
            Gracetimer = new BasicTimer(graceTime);
            //Debug.Log("NEW VELOCITY: " + agnes.velocity);
            timer = new BasicTimer(0.5f);

            if (CurrentToughness <= 0)
            {
                if (healthMeter != null && healthMeterBackground != null)
                {
                    healthMeter.enabled = false;
                    healthMeterBackground.enabled = false;
                }
                EventSystem.Current.FireEvent(new EnemyDieEvent("Bonde died", gameObject));
                int randomNr = Random.Range(0, 2);
                float volume = 1f;
                if (randomNr == 2)
                    volume = 0.3f;
                EventSystem.Current.FireEvent(new PlaySoundEvent(transform.position, deathScreams[randomNr], volume, 0.9f, 1.1f));
            }
            else
            {
                EventSystem.Current.FireEvent(new PlaySoundEvent(transform.position, hitSound, 0.6f, 0.9f, 1.3f));
            }

            if (healthMeter != null)
            {
                healthMeter.fillAmount = CurrentToughness / toughness;
            }
        }
    }

    public void getCrushed()
    {
        isDying = true;
        EventSystem.Current.FireEvent(new PlaySoundEvent(transform.position, hitSound, 0.6f, 0.9f, 1.3f));
    }
}
