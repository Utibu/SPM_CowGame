using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioListener : MonoBehaviour
{
    private AudioSource auSource;
    private BasicTimer soundTimer;
    [SerializeField] private float audioDelay;


    // Start is called before the first frame update
    void Start()
    {
        soundTimer = new BasicTimer(audioDelay);
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
        transform.position = SoundEvent.position;
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
