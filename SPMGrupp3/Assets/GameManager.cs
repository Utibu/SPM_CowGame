using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    [HideInInspector] public Transform currentCheckpoint;
    public int deathCount = 0;
    public PlayerStateMachine player;
    public Transform originalSpawnTransform;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        QualitySettings.vSyncCount = 0;  // VSync must be disabled
        Application.targetFrameRate = 45;

    }
    // Start is called before the first frame update
    void Start()
    {
        EventSystem.Current.RegisterListener<OnPlayerDiedEvent>(Respawn);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CheckpointTaken(Transform checkPointTransform)
    {
        currentCheckpoint = checkPointTransform.Find("SpawnPoint");
        checkPointTransform.gameObject.SetActive(false);
    }

    void Respawn(OnPlayerDiedEvent eventInfo)
    {
        deathCount++;
        if(deathCount <= 3 && currentCheckpoint != null)
        {
            player.Respawn(currentCheckpoint.transform.position);
        } else
        {
            player.Respawn(originalSpawnTransform.position);
        }
    }

    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }
}
