using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeRampButton : MonoBehaviour
{
    public GameObject gameObjectToHide;
    public GameObject gameObjectToShow;
    public Camera cutSceneCamera;
    public Camera originalCamera;
    public int timeInCutScene = 2;
    // Start is called before the first frame update
    void Start()
    {
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

    public void Act()
    {
        gameObjectToHide.SetActive(false);
        gameObjectToShow.SetActive(true);
        /*if(cutSceneCamera != null)
        {
            cutSceneCamera.enabled = true;
            originalCamera.enabled = false;
            Invoke("TurnOnCamera", timeInCutScene);
        }*/
    }

    public void TurnOnCamera()
    {
        originalCamera.enabled = true;
        cutSceneCamera.enabled = false;
    }
}
