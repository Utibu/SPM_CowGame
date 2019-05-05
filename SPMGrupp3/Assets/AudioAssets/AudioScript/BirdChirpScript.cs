using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdChirpScript : MonoBehaviour
{

    private AudioSource source;
    public AudioClip clip;
    // Use this for initialization
    void Start()
    {
        source = GetComponent<AudioSource>();
        source.clip = clip;
        source.loop = true;

        source.time = Random.Range(0, clip.length);

        source.Play();
    }
}


