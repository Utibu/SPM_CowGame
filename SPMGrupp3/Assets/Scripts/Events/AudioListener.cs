using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioListener : MonoBehaviour
{
    [SerializeField] private GameObject audioPlayer;
    [SerializeField] private int audioPlayerCount;
    private GameObject obj;
    private int count = 0;
    private bool audioPlayersAvailable;
    private List<AudioSource> audioPlayerList = new List<AudioSource>();


    void Start()
    {
        EventSystem.Current.RegisterListener<PlaySoundEvent>(EmitSound);
        count = 0;
        audioPlayerList.Clear();
        while (count <= audioPlayerCount)
        {
            obj = Instantiate(audioPlayer);
            audioPlayerList.Add(obj.GetComponent<AudioSource>());
            count++;
        }
    }

    private void Awake()
    {
        
    }

    private void EmitSound(PlaySoundEvent SoundEvent)
    {
        audioPlayersAvailable = false;
        foreach(AudioSource audioSource in audioPlayerList)
        {
            audioPlayersAvailable = false;
            if(audioSource.isPlaying != true && audioSource.gameObject != null)
            {
                audioSource.gameObject.transform.position = SoundEvent.position;
                audioSource.pitch = Random.Range(SoundEvent.pitchMin, SoundEvent.pitchMax);
                audioSource.volume = SoundEvent.volume;
                audioSource.PlayOneShot(SoundEvent.sound);
                audioPlayersAvailable = true;
                return;
            }
        }

        if(audioPlayersAvailable == false)
        {
            Debug.Log("new sound");
            obj = Instantiate(audioPlayer, SoundEvent.position, Quaternion.identity);
            obj.GetComponent<AudioSource>().pitch = Random.Range(SoundEvent.pitchMin, SoundEvent.pitchMax);
            obj.GetComponent<AudioSource>().volume = SoundEvent.volume;
            obj.GetComponent<AudioSource>().PlayOneShot(SoundEvent.sound);
            Destroy(obj, SoundEvent.sound.length);
        }
    }
}
