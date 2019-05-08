using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MakeRampButton : ButtonScript
{

    [SerializeField]private bool isActive = false;
    
    public GameObject gameObjectToHide;
    public GameObject gameObjectToShow;
    public Camera cutSceneCamera;
    public Camera originalCamera;
    public int timeInCutScene = 2;
    public AudioClip errorSound;
    private AudioSource AudioSrc;
    public Canvas descriptorCanvas;
    public Text descriptorText;

   
    // Start is called before the first frame update
    void Start()
    {
        descriptorCanvas.gameObject.SetActive(false);
        AudioSrc = GetComponent<AudioSource>();

        gameObjectToHide.SetActive(true);
        gameObjectToShow.SetActive(false);
        originalCamera = Camera.main;
        if (cutSceneCamera != null)
        {
            cutSceneCamera.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Act()
    {
        Debug.Log("ramp button is acting");
        descriptorCanvas.gameObject.SetActive(true);
        //knappen har ström
        if (isActive)
        {
            Debug.Log("and is active.");
            gameObjectToHide.SetActive(false);
            gameObjectToShow.SetActive(true);
            descriptorText.text = "I think I heard something in the room before!";
            /*if(cutSceneCamera != null)
            {
                cutSceneCamera.enabled = true;
                originalCamera.enabled = false;
                Invoke("TurnOnCamera", timeInCutScene);
            }*/
        }

        // knappen har inte ström
        //AudioSource.PlayClipAtPoint(errorSound, transform.position);
        // visa bild på display


    }

    public void TurnOnCamera()
    {
        originalCamera.enabled = true;
        cutSceneCamera.enabled = false;
    }

    public void SetActive()
    {
        Debug.Log("RAMP IS ACTIVATEBELLELEFDGFRGFG");
        isActive = true;
    }

    public void SetUnactive()
    {
            isActive = false;
    }
}
