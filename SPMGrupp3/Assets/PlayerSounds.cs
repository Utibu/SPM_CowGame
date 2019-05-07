using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSounds : MonoBehaviour
{

    public Text countText;
    public Text winText;

    private int clipIndex;
    private int count;

    private AudioSource[] sources;
    private AudioSource source1;
    private AudioSource source2;

    public AudioClip[] pick_up_sounds;
    public AudioClip dash_hit_sound;

    private float volLowRange = 0.2f;
    private float volHighRange = 1.0f;
    private float lowPitchRange = 0.95f;
    private float highPitchRange = 1.05f;

    // Start is called before the first frame update
    void Start()
    {
        sources = GetComponents<AudioSource>();
        source1 = sources[0];
        source2 = sources[1];
    }

    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Pick Up"))
        {
            other.gameObject.SetActive(false);
            count++;
            SetCountText();

            clipIndex = Random.Range(0, pick_up_sounds.Length);
            source1.PlayOneShot(pick_up_sounds[clipIndex]);
        }
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        if (count >= 14)
        {
            // Här ska vi se till att om spelaren har plockat upp X antal mynt så ska dennes HP höjas
            //winText.text = "You win!"; 
            //source1.PlayOneShot(win_sound);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Dashable"))
        {

            source1.pitch = Random.Range(lowPitchRange, highPitchRange);
            float vol = Random.Range(volLowRange, volHighRange);
            source1.PlayOneShot(dash_hit_sound, vol);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
