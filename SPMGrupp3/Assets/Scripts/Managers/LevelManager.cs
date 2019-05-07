using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance = null;
    [HideInInspector] public Transform currentCheckpoint;
    public Transform originalSpawnTransform;
    private List<Transform> checkpoints = new List<Transform>();

    public float normalJumpForce;
    public float dashJumpForce;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
        {
            instance.originalSpawnTransform = originalSpawnTransform;
        }

        checkpoints.Clear();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!GameManager.instance.debug)
        {
            Debug.Log("hello");
            GameManager.instance.player.transform.position = instance.originalSpawnTransform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RegisterCheckpointTaken(Transform checkPointTransform)
    {
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
