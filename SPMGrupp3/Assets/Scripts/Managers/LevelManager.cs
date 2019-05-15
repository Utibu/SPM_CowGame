using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance = null;
    [HideInInspector] public Transform currentCheckpoint;
    public Transform originalSpawnTransform;
    private List<Transform> checkpoints = new List<Transform>();
    public bool hasGateKey = false;
    public float normalJumpForce;
    public float dashJumpForce;
    public int pickedCoins = 0;
    public AudioClip checkpointSound;

    public float playerScale = 1f;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        checkpoints.Clear();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!GameManager.instance.debug)
        {
            GameManager.instance.player.transform.position = originalSpawnTransform.position;
        }

        GameManager.instance.player.transform.localScale = Vector3.one * playerScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RegisterCheckpointTaken(Transform checkPointTransform)
    {
        EventSystem.Current.FireEvent(new PlaySoundEvent(checkPointTransform.position, checkpointSound, 1f, 1f,1f));
        currentCheckpoint = checkPointTransform.Find("SpawnPoint");
        checkPointTransform.gameObject.SetActive(false);

    }

    public void RegisterCheckpoint(Transform point)
    {
        checkpoints.Add(point);
    }

    public void ResetCheckpoints()
    {
        foreach(Transform t in checkpoints)
        {
            t.gameObject.SetActive(true);
        }
    }
}
