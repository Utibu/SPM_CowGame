﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dashable : MonoBehaviour
{
    public float requiredLevel = 1;
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
        if(destructionSounds.Length == 0)
        {
            return null;
        }
        else
        {
            return destructionSounds[Random.Range(0, destructionSounds.Length)];
        }
    }
}
