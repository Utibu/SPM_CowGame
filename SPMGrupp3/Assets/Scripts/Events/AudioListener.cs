using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioListener : MonoBehaviour
{
    [SerializeField] private GameObject audioPlayer;
    private GameObject obj;
    [SerializeField] private int audioPlayerCount;
    private int count = 0;
    private List<AudioSource> audioPlayerList = new List<AudioSource>();
    private bool audioPlayersAvailable;


    void Start()
    {
        EventSystem.Current.RegisterListener<PlaySoundEvent>(EmitSound);
    }

    private void Awake()
    {
        while(count <= audioPlayerCount)
        {
            obj = Instantiate(audioPlayer);
            audioPlayerList.Add(obj.GetComponent<AudioSource>());
            count++;
        }
    }

    private void EmitSound(PlaySoundEvent SoundEvent)
    {
        foreach(AudioSource audioSource in audioPlayerList)
        {
            audioPlayersAvailable = false;
            if(audioSource.isPlaying != true)
            {
                audioSource.gameObject.transform.position = SoundEvent.position;
                audioSource.pitch = Random.Range(SoundEvent.pitchMin, SoundEvent.pitchMax);
                audioSource.volume = SoundEvent.volume;
                audioSource.PlayOneShot(SoundEvent.sound);
                audioPlayersAvailable = true;
                return;
            }
        }

        if(audioPlayersAvailable != true)
        {
            Debug.Log("instantiating new player");
            obj = Instantiate(audioPlayer, SoundEvent.position, Quaternion.identity);
            obj.GetComponent<AudioSource>().pitch = Random.Range(SoundEvent.pitchMin, SoundEvent.pitchMax);
            obj.GetComponent<AudioSource>().volume = SoundEvent.volume;
            obj.GetComponent<AudioSource>().PlayOneShot(SoundEvent.sound);
            Destroy(obj, SoundEvent.sound.length);
        }
        /*
        Debug.Log("instantiating new player");
        obj = Instantiate(audioPlayer, SoundEvent.position, Quaternion.identity);
        obj.GetComponent<AudioSource>().pitch = Random.Range(SoundEvent.pitchMin, SoundEvent.pitchMax);
        obj.GetComponent<AudioSource>().volume = SoundEvent.volume;
        obj.GetComponent<AudioSource>().PlayOneShot(SoundEvent.sound);
        Destroy(obj, SoundEvent.sound.length);
        */
        /*
        obj = Instantiate(audioPlayer, SoundEvent.position, Quaternion.identity);
        obj.GetComponent<AudioSource>().pitch = Random.Range(SoundEvent.pitchMin, SoundEvent.pitchMax);
        obj.GetComponent<AudioSource>().volume = SoundEvent.volume;
        obj.GetComponent<AudioSource>().PlayOneShot(SoundEvent.sound);
        Destroy(obj, SoundEvent.sound.length);
        */

        /*
        transform.position = SoundEvent.position;
        auSource.pitch = Random.Range(SoundEvent.pitchMin, SoundEvent.pitchMax);
        auSource.volume = SoundEvent.volume;
        if(soundTimer.IsCompleted(Time.deltaTime, false))
        {
            //Debug.Log("playing sound");
            auSource.PlayOneShot(SoundEvent.sound);
            soundTimer.Reset();
        }
        */
    }
}
