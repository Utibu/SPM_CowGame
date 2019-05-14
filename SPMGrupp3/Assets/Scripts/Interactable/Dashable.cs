using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dashable : MonoBehaviour
{
    public float requiredMagnitude = 10;
    [SerializeField] private AudioClip[] destructionSounds;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public AudioClip GetClip()
    {
        return destructionSounds[Random.Range(0, destructionSounds.Length)];
    }
}
