using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathMessageContainer : MonoBehaviour
{


    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Show()
    {
        gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void RespawnButtonClicked()
    {

    }

    public void MainMenuButton()
    {

    }
}
