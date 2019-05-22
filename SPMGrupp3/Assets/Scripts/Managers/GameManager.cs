﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    //public int deathCount = 0;
    public PlayerStateMachine player;
    public Text velocityText;
    public Text coinCountText;
    [HideInInspector] public int coinCount;
    [SerializeField] private int coinsToHPIncrease = 20;
    [SerializeField] private int HPIncreaseAmount = 20;
    public bool debug;
    public InputManager inputManager;
    public bool showCursor;
    //public Canvas UI;
    public Camera cam;
    public GameObject controlsUI;
    private bool isLoadingScene = false;
    private bool isPaused = false;

    private Vector3 horizontalSpeed = new Vector3();
    //private AudioSource auSource;

    void Awake()
    {

        if (instance == null)
            instance = this;
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        inputManager = new InputManager();
        //auSource = GetComponent<AudioSource>();

        EventSystem.Current.RegisterListener<OnPlayerDiedEvent>(Respawn);
        //EventSystem.Current.RegisterListener<PlaySoundEvent>(EmitSound);
        

    }
    // Start is called before the first frame update
    void Start()
    {
        controlsUI.SetActive(true);
        Invoke("HideControlsUI", 6f);

        if (!showCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

    }
    
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            Pause();
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            Resume();
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            LoadScene(2);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //LoadMenu();
            if(isPaused)
            {
                Resume();
                UIManager.instance.HideMenu();
            } else
            {
                Pause();
                UIManager.instance.ShowMenu();
            }
        }

        UpdateUIText();
        CheckCoins();
    }

    public void Pause()
    {
        EventSystem.Current.FireEvent(new PauseEvent(""));
        isPaused = true;
    }

    public void Resume()
    {
        EventSystem.Current.FireEvent(new ResumeEvent(""));
        isPaused = false;
    }

    private void CheckCoins()
    {
        if (coinCount >= coinsToHPIncrease)
        {
            player.playerValues.maxHealth += HPIncreaseAmount;
            player.playerValues.health = player.playerValues.maxHealth;
            coinCount = 0;
            LevelManager.instance.pickedCoins = 0;
        }
    }

    private void UpdateUIText()
    {
        velocityText.text = "Velocity: " + player.velocity.magnitude;
        coinCountText.text = "Coins: " + coinCount;
        //livesText.text = "Lives: " + (3 - deathCount);
    }

    void HideControlsUI()
    {
        controlsUI.SetActive(false);
    }

    void Respawn(OnPlayerDiedEvent eventInfo)
    {
        /*deathCount++;
        if(deathCount <= 3)
        {
            
            
        } else
        {
            deathCount = 0;
            LoadScene(SceneManager.GetActiveScene().buildIndex);
            coinCount -= LevelManager.instance.pickedCoins;
        }*/
        UIManager.instance.ShowDeathMessage();

        //if (LevelManager.instance.currentCheckpoint != null)
        //{
            //player.Respawn(LevelManager.instance.currentCheckpoint.transform.position);
        //}
        //else
        //{
            //player.Respawn(LevelManager.instance.originalSpawnTransform.position);
        //}

    }

    public void LoadScene(int index)
    {
        RemoveListeners();
        if(!isLoadingScene)
        {
            IEnumerator coroutine = LoadSceneRoutine(index);
            isLoadingScene = true;
            StartCoroutine(coroutine);
        }
        
    }

    public void LoadMenu()
    {
        StartCoroutine("LoadMenuRoutine");
    }

    public void RemoveListeners()
    {
        EventSystem.Current.ClearListener<EnemyDieEvent>();
        EventSystem.Current.FireEvent(new UnregisterListenerEvent("Unregister events, prepare for levelswitching"));
        EventSystem.Current.ClearListener<UnregisterListenerEvent>();
        //EventSystem.Current.ClearListener<PauseEvent>();
        //EventSystem.Current.ClearListener<ResumeEvent>();
    }

    IEnumerator LoadMenuRoutine()
    {
        var loading = SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
        yield return loading;
        //var unloading = SceneManager.UnloadSceneAsync(3);
        //yield return unloading;


        var scene = SceneManager.GetSceneByBuildIndex(0);
        SceneManager.SetActiveScene(scene);
        isLoadingScene = false;
    }

    IEnumerator LoadSceneRoutine(int index)
    {
        var unloading = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        yield return unloading;
        var loading = SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);
        yield return loading;
        
        var scene = SceneManager.GetSceneByBuildIndex(index);
        SceneManager.SetActiveScene(scene);
        isLoadingScene = false;
    }

    /*
    // inte nödvändigtvis permanent plats för sound events. 
    private void EmitSound(PlaySoundEvent SoundEvent)
    {
        auSource.pitch = Random.Range(SoundEvent.pitchMin, SoundEvent.pitchMax);
        auSource.volume = SoundEvent.volume;
        auSource.PlayOneShot(SoundEvent.sound);
    }
    */
}
