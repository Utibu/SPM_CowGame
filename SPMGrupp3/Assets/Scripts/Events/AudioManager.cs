using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private GameObject audioPlayer;
    [SerializeField] private int audioPlayerCount;
    private GameObject obj;
    private int count = 0;
    private bool audioPlayersAvailable;
    private List<AudioSource> audioPlayerList = new List<AudioSource>();


    private void Start()
    {
        EventSystem.Current.RegisterListener<PlaySoundEvent>(EmitSound);
        //OnLevelLoaded();
        count = 0;
        audioPlayerList.Clear();
        Debug.LogWarning("CREATING NEW LIST");
        while (count <= audioPlayerCount)
        {
            obj = Instantiate(audioPlayer, this.transform);
            audioPlayerList.Add(obj.GetComponent<AudioSource>());
            count++;
        }
    }
    
    public void OnLevelLoaded()
    {
        /*
        count = 0;
        audioPlayerList.Clear();
        Debug.LogWarning("CREATING NEW LIST");
        while (count <= audioPlayerCount)
        {
            obj = Instantiate(audioPlayer);
            audioPlayerList.Add(obj.GetComponent<AudioSource>());
            count++;
        }
        */
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

        if(audioPlayersAvailable == false)
        {
            obj = Instantiate(audioPlayer, SoundEvent.position, Quaternion.identity);
            obj.GetComponent<AudioSource>().pitch = Random.Range(SoundEvent.pitchMin, SoundEvent.pitchMax);
            obj.GetComponent<AudioSource>().volume = SoundEvent.volume;
            obj.GetComponent<AudioSource>().PlayOneShot(SoundEvent.sound);
            Destroy(obj, SoundEvent.sound.length);
        }
    }
}
