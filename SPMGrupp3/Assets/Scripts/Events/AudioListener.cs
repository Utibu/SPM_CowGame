﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioListener : MonoBehaviour
{
    private AudioSource auSource;
    private BasicTimer soundTimer = new BasicTimer(3f);

    // Start is called before the first frame update
    void Start()
    {
        EventSystem.Current.RegisterListener<PlaySoundEvent>(EmitSound);
    }

    private void Awake()
    {
        auSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        soundTimer.Update(Time.deltaTime);
        
    }

    private void EmitSound(PlaySoundEvent SoundEvent)
    {
        auSource.pitch = Random.Range(SoundEvent.pitchMin, SoundEvent.pitchMax);
        auSource.volume = SoundEvent.volume;
        if(soundTimer.IsCompleted(Time.deltaTime, false))
        {
            Debug.Log("playing sound");
            auSource.PlayOneShot(SoundEvent.sound);
            soundTimer.Reset();
        }
    }
}
