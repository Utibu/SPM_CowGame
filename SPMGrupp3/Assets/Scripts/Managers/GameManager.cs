using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public int deathCount = 0;
    public PlayerStateMachine player;
    public Text velocityText;
    public Image dashCooldownImage;
    public Image dashSpeedImage;
    public Text coinCountText;
    [HideInInspector] public int coinCount;
    [SerializeField] private int coinsToHPIncrease = 20;
    public bool debug;
    public InputManager inputManager;
    public bool showCursor;
    public Canvas UI;
    public Camera cam;
    public GameObject controlsUI;
    public Text livesText;
    private bool isLoadingScene = false;

    private Vector3 horizontalSpeed = new Vector3();

    void Awake()
    {
        Debug.Log("in awake");

        if (instance == null)
            instance = this;
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        Debug.Log(debug);

        inputManager = new InputManager();

        EventSystem.Current.RegisterListener<OnPlayerDiedEvent>(Respawn);
        

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
        
        if(velocityText != null)
        {
            velocityText.text = "Velocity: " + player.velocity.magnitude;
        }

        if(coinCountText != null)
        {

            coinCountText.text = "Coins: " + coinCount;
        }

        if(livesText != null)
        {
            livesText.text = "Lives: " + (3 - deathCount);
        }

        if(dashCooldownImage != null)
        {
            if(player.allowedToDash)
            {
                dashCooldownImage.fillAmount = 1;
            } else
            {
                dashCooldownImage.fillAmount = (player.elapsedDashTime) / player.dashCooldown;
            }
            
        }


        if(dashSpeedImage != null)
        {
            //horizontalSpeed = new Vector3(player.velocity.x, 0.0f, player.velocity.z);
            //dashSpeedImage.fillAmount = player.velocity.magnitude / player.maxSpeed;
            dashSpeedImage.fillAmount = player.velocity.magnitude / player.terminalVelocity;
            //if (player.velocity.magnitude > player.GetComponent<DashState>().toSuperDash)
            //{
            //  dashSpeedImage.color = Color.red;
            //}
            if (player.GetCurrentState().GetType() == typeof(AirState))
            {
                dashSpeedImage.fillAmount = 0;
            }

            if (player.velocity.magnitude > player.toSuperDash)
            {
                dashSpeedImage.color = Color.red;
            } else if (player.velocity.magnitude > player.velocityToDash)
            {
                dashSpeedImage.color = Color.yellow;
            } else
            {
                dashSpeedImage.color = Color.green;
            }
        }

        if(coinCount >= coinsToHPIncrease)
        {
            player.playerValues.maxHealth += 20;
            player.playerValues.health = player.playerValues.maxHealth;
            coinCount = 0;
        }
    }

    void HideControlsUI()
    {
        controlsUI.SetActive(false);
    }

    void Respawn(OnPlayerDiedEvent eventInfo)
    {
        Debug.Log("RESPAWN DC " + deathCount);
        deathCount++;
        if(deathCount <= 3)
        {
            if(LevelManager.instance.currentCheckpoint != null)
            {
                Debug.Log("OO:" + player.name);
                player.Respawn(LevelManager.instance.currentCheckpoint.transform.position);
            } else
            {
                Debug.Log("NN:" + player.name);
                player.Respawn(LevelManager.instance.originalSpawnTransform.position);
            }
            
        } else
        {
            deathCount = 0;
            LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

    }

    public void LoadScene(int index)
    {
        if(!isLoadingScene)
        {
            IEnumerator coroutine = LoadSceneRoutine(index);
            isLoadingScene = true;
            StartCoroutine(coroutine);
        }
        
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

}
