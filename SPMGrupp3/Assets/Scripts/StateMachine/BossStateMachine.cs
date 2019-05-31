using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossStateMachine : Peasant
{
    public GameObject bullet;
    [HideInInspector] public GameObject ActiveWeapon;
    [SerializeField] private GameObject pistol;
    [SerializeField] private GameObject rifle;
    public float attackSpeed;
    public float bulletsBeforeReload;
    [HideInInspector]
    public float bulletsShotSinceReload;
    public AudioClip GunSound;
    public AudioClip ReloadSound;

    public float health = 100f;
    public float damageOnDash = 25f;
    public bool isInvincible = false;

    public GameObject underlingPrefab;
    public GameObject underlingSpawnArea;
    [SerializeField] private float spawnRadius;
    public GameObject snipeLocation;
    public Vector3 Destination;

    private float timeSinceLastHit = 0f;
    public float timeBetweenSpawns = 0.2f;
    public int underlingQuantityPerWave = 4;
    [HideInInspector] public int count = 0;
    private List<GameObject> underlingList = new List<GameObject>();
    private List<GameObject> allUnderlings = new List<GameObject>();
    //Gör private
    public Vector3 originalPosition;

    public Image healthBar;
    public Image reloadMeter;
    [HideInInspector] public MeshRenderer renderColor;
    [HideInInspector] public LineRenderer LaserSightRenderer;


    protected override void Awake()
    {
        base.Awake();
        CurrentToughness = toughness;
        LaserSightRenderer = rifle.GetComponentInChildren<LineRenderer>();
    }

    public override void Start()
    {
        base.Start();
        originalPosition = transform.position;
        EventSystem.Current.RegisterListener<EnemyDieEvent>(OnUnderlingDeath);
        EventSystem.Current.RegisterListener<OnPlayerDiedEvent>(OnPlayerDeath);
        renderColor = GetComponent<MeshRenderer>();
        ActiveWeapon = pistol;
    }

    public override void UnregisterEnemy()
    {
        base.UnregisterEnemy();
        EventSystem.Current.UnregisterListener<OnPlayerDiedEvent>(OnPlayerDeath);
    }

    public override void Update()
    {
        base.Update();
    }

    public override void PlayerDash(Vector3 velocity, bool useKnockback = true)
    {
        //base.PlayerDash(velocity);
        if (Gracetimer == null && isInvincible == false)
        {
            CurrentToughness -= 1;
            if (CurrentToughness <= 0)
            {
                Debug.Log("Boss dead");
                Destroy(gameObject);
                UIManager.instance.ShowVictoryMessage();
                GameManager.instance.RemoveSave();
                return;
            }
            transform.position += transform.forward * -2;
            timeSinceLastHit = 0;
            count = 0;
            SpawnUnderling();
            Destination = snipeLocation.transform.position;
            Transition<BossTransitionState>();
        }
        
        
        healthBar.fillAmount =  CurrentToughness / toughness;
        /*
        Debug.Log("Current: " + CurrentToughness + " Total: " + toughness + " " + CurrentToughness/toughness);
        */


    }

    public void SpawnUnderling()
    {
        count++;
        GameObject underling = Instantiate(underlingPrefab, (underlingSpawnArea.transform.position), Quaternion.identity);
        Peasant bonde = underling.GetComponent<Peasant>();
        bonde.ShouldSaveEnemy = false;
        bonde.patrolPoints = patrolPoints;
        bonde.maxVisibility = 25f;
        underlingList.Add(underling);
        allUnderlings.Add(underling);
        if (count < underlingQuantityPerWave)
        {
            Invoke("SpawnUnderling", timeBetweenSpawns);
        }

    }

    private void OnUnderlingDeath(EnemyDieEvent enemyDeath)
    {
        if (underlingList.Contains(enemyDeath.enemy))
        {
            underlingList.Remove(enemyDeath.enemy);
            Debug.Log(underlingList.Count);


            if (underlingList.Count == 0)
            {
                Destination = originalPosition;
                Transition<BossTransitionState>();
                
            }
        }
    }

    private void OnPlayerDeath(OnPlayerDiedEvent playerDeadEvent)
    {
        Debug.Log("BOSS RESET! **********************************");
        // reset hp
        CurrentToughness = toughness;
        healthBar.fillAmount = CurrentToughness / toughness;

        // put boss at original position
        Destination = originalPosition;
        Transition<BossTransitionState>();

        // remove spawnlings
        foreach (GameObject underling  in allUnderlings)
        {
            underling.GetComponent<Peasant>().UnregisterEnemy();
            Destroy(underling, 2f); // 2 sec delay, to not disrupt iteration(?)
            Debug.Log("foreach underling");
        }
        underlingList.Clear();
        allUnderlings.Clear();
    }

    public void ToggleActiveWeapon()
    {
        if (ActiveWeapon.Equals(pistol))
        {
            pistol.SetActive(false);
            rifle.SetActive(true);
            ActiveWeapon = rifle;
        }
        else
        {
            pistol.SetActive(true);
            rifle.SetActive(false);
            ActiveWeapon = pistol;
        }
    }

}
