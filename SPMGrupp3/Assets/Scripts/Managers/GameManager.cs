using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    //[HideInInspector] public Transform currentCheckpoint;
    public int deathCount = 0;
    public PlayerStateMachine player;
    //public Transform originalSpawnTransform;
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

    private Vector3 horizontalSpeed = new Vector3();

    void Awake()
    {
        Debug.Log("in awake");

        if (instance == null)
            instance = this;
        else if (instance != this)
        {
            //Debug.Log(originalSpawnTransform);
            //instance.originalSpawnTransform = originalSpawnTransform;
            
            Destroy(player.gameObject);           
            Destroy(UI.gameObject);
            Destroy(cam.gameObject);
            Destroy(gameObject);
        }

        Debug.Log(debug);
        //Debug.Log(instance.originalSpawnTransform);

        inputManager = new InputManager();

        if (!debug)
        {
            Debug.Log("hello");
           // instance.player.transform.position = instance.originalSpawnTransform.position;
        }

        //QualitySettings.vSyncCount = 0;  // VSync must be disabled
        //Application.targetFrameRate = 45;

    }
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("in start");

        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(UI.gameObject);
        DontDestroyOnLoad(player.gameObject);
        DontDestroyOnLoad(cam.gameObject);
        
        EventSystem.Current.RegisterListener<OnPlayerDiedEvent>(Respawn);
        
        
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
            //Debug.Log(player.velocity.magnitude);
            //Debug.Log(velocityText.text);
        }

        if(coinCountText != null)
        {

            coinCountText.text = "Coins: " + coinCount;
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
            }
            
            //else if()
            else
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

    

    void Respawn(OnPlayerDiedEvent eventInfo)
    {
        deathCount++;
        if(deathCount <= 3 && LevelManager.instance.currentCheckpoint != null)
        {
            player.Respawn(LevelManager.instance.currentCheckpoint.transform.position);
        } else
        {
            player.Respawn(LevelManager.instance.originalSpawnTransform.position);
            LevelManager.instance.ResetCheckpoints();
        }

    }

    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }
}
