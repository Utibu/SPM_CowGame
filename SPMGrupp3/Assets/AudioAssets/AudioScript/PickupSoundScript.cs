using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSoundScript : MonoBehaviour
{

    private AudioSource[] sources;
    private AudioSource source1;
    public AudioClip[] pick_up_sounds;

    // Start is called before the first frame update
    void Start()
    {
        sources = GetComponents<AudioSource>();
        source1 = sources[0];
    }

    if (other.gameObject.CompareTag ("Droppable") ){
			other.gameObject.SetActive(false);
			count++;
			SetCountText();

    clipIndex = Random.Range(0, pick_up_sounds.Length);
            source1.PlayOneShot(pick_up_sounds[clipIndex]);

    // Update is called once per frame
    void Update()
    {
        
    }
}
