//Main Author: Niklas Almqvist
//Secondary Author: Joakim Ljung, Sofia Kauko
using System.Collections;
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
    [HideInInspector] public int totalCoinCount;
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
    public SaveManager SaveManager;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private AudioClip hpIncreaseSound;
    public AudioManager AudioManager { get { return audioManager; } set { audioManager = value; } }

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
        /*
            START: Just for debugs
         */
        /*if(Input.GetKeyDown(KeyCode.P))
        {
            Pause();
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            Resume();
        }

        if(Input.GetKeyDown(KeyCode.N))
        {
            RemoveSave();
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            LoadScene(2);
        }*/
        /*
            END: Just for debugs
         */

        if (GameManager.instance.inputManager.MenuButtonDown())
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
            EventSystem.Current.FireEvent(new PlaySoundEvent(player.transform.position, hpIncreaseSound, 1f, 0.9f, 1.1f));
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
    }

    void HideControlsUI()
    {
        controlsUI.SetActive(false);
    }

    void Respawn(OnPlayerDiedEvent eventInfo)
    {
        player.PlayerSounds.SetPlayerFootstepsSound(FootstepsState.None); // så man inte spelar galopp ljuden när man e dö.
        UIManager.instance.ShowDeathMessage();
    }

    public void RemoveSave()
    {
        SaveManager.RemoveSave();
    }

    public void LoadScene(int index, bool checkSaved = false)
    {
        GameManager.instance.SaveManager.ClearSaves();
        RemoveListeners();
        /*if(checkSaved)
        {
            SaveModel save = Helper.GetSaveFile();
            if(save != null && save.OnLevel == index)
            {
                GameInformation.ShouldContinue = true;
            }
        }*/

        if (!isLoadingScene)
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
        GameInformation.OnLevel = 0;
        isLoadingScene = false;
    }

    IEnumerator LoadSceneRoutine(int index)
    {
        UIManager.instance.LoadingImage.gameObject.SetActive(true);
        UIManager.instance.LoadingBar.fillAmount = 0f;

        AsyncOperation unloading = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        yield return unloading;
        AsyncOperation loading = SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);
        while (!loading.isDone)
        {
            UIManager.instance.LoadingBar.fillAmount = loading.progress;

            if (loading.progress >= 0.9f)
            {
                loading.allowSceneActivation = true;
                
                //UIManager.instance.LoadingBar.gameObject.SetActive(false);
            }

            yield return null;
        }

        var scene = SceneManager.GetSceneByBuildIndex(index);
        SceneManager.SetActiveScene(scene);
        GameInformation.OnLevel = index;
        foreach (GameObject go in scene.GetRootGameObjects())
        {
            if (go.CompareTag("LevelManager"))
            {
                go.GetComponent<LevelManager>().LoadGame();
            }
        }
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
