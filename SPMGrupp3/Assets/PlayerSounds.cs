using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Author: Alexander Zingmark
// Co-Author: Niklas Almqvist

public enum FootstepsState { Normal, Dash, None };

public class PlayerSounds : MonoBehaviour
{

    private int clipIndex;
    private int count;

    private AudioSource[] sources;
    private AudioSource source1;
    //private AudioSource source2;

    public AudioClip[] pick_up_sounds;
    public AudioClip dash_hit_sound;
    [SerializeField] private AudioClip normalFootstepsSound;
    [SerializeField] private AudioClip dashFootstepsSound;
    [SerializeField] private AudioSource footstepsAudioSource;

    private float volLowRange = 0.2f;
    private float volHighRange = 1.0f;
    private float lowPitchRange = 0.95f;
    private float highPitchRange = 1.05f;

    // Start is called before the first frame update
    void Start()
    {
        //sources = GetComponents<AudioSource>();
        //source1 = sources[0];
        //source2 = sources[1];
    }

    public void PlayTriggerSound(Collider other)
    {
        
        /*
        if (other.GetComponent<CoinPickup>() != null)
        {
            //other.gameObject.SetActive(false);
            //count++;
            //SetCountText();
            Debug.Log("COIN SOUND");
            clipIndex = Random.Range(0, pick_up_sounds.Length);
            source1.PlayOneShot(pick_up_sounds[clipIndex]);
        }
        */
    }

    public AudioClip GetCurrentFootstepsClip()
    {
        return footstepsAudioSource.clip;
    }

    public void SetPlayerFootstepsSound(FootstepsState state)
    {
        switch(state)
        {
            case FootstepsState.Normal:
                footstepsAudioSource.clip = normalFootstepsSound;
                //Debug.Log("NORMAL SOUNDS");
                break;
            case FootstepsState.Dash:
                footstepsAudioSource.clip = dashFootstepsSound;
                //Debug.Log("DASH SOUNDS");
                break;
            default:
                footstepsAudioSource.clip = null;
                footstepsAudioSource.Stop();
                //Debug.Log("NO SOUNDS");
                break;
        }

        if(footstepsAudioSource.clip != null && footstepsAudioSource.isPlaying == false)
        {
            footstepsAudioSource.Play();
        }
    }

    public void PlayCollisionSound(Collider collision)
    {
        if (collision.gameObject.CompareTag("Dashable"))
        {

            //source1.pitch = Random.Range(lowPitchRange, highPitchRange);
            //float vol = Random.Range(volLowRange, volHighRange);
            //source1.PlayOneShot(dash_hit_sound, vol);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
