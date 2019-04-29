using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSpawner : MonoBehaviour
{

    public GameObject rockPrefab;
    public float secondsBetweenSpawns;
    private float time = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(time % 60 > secondsBetweenSpawns) {
            time = 0.0f;
            GameObject go = Instantiate(rockPrefab, this.transform.position, Quaternion.identity);
        } else {
            time += Time.deltaTime;
        }
    }
}
