//Main Author: Joakim Ljung
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTransitioner : Triggable
{
    [SerializeField] private GameObject audioSystem;
    [SerializeField] private AudioClip newMusic;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = audioSystem.GetComponent<AudioSource>();
    }

    public override void OnPlayerTriggerEnter(Collider hitCollider)
    {
        StartCoroutine(FadeOut(audioSource, 2f));
        GetComponent<BoxCollider>().enabled = false;
    }

    //Code from : https://forum.unity.com/threads/fade-out-audio-source.335031/
    public IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
        audioSource.clip = newMusic;
        audioSource.Play();
    }
}
