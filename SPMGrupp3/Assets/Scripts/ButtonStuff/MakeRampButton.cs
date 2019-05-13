﻿using System.Collections;
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
    public AudioClip turnOnSound;
    public Canvas descriptorCanvas;
    public Text descriptorText;

   
    // Start is called before the first frame update
    void Start()
    {
        descriptorCanvas.gameObject.SetActive(false);

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
        descriptorCanvas.gameObject.SetActive(true);
        // för testing. ljud ska emittas var gång knapp interageras med oavsett om den är på eller ej.
        EventSystem.Current.FireEvent(new PlaySoundEvent(transform.position, gameObject, turnOnSound));

        //knappen har ström
        if (isActive)
        {
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
        

    }

    public void TurnOnCamera()
    {
        originalCamera.enabled = true;
        cutSceneCamera.enabled = false;
    }

    public void SetActive()
    {
        EventSystem.Current.FireEvent(new PlaySoundEvent(transform.position, gameObject, turnOnSound));
        isActive = true;
    }

    public void SetUnactive()
    {
            isActive = false;
    }
}
