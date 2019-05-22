using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathMessageContainer : MonoBehaviour
{
    //[SerializeField] private PlayerStateMachine player;
    //private PlayerStateMachine stateMachine;
    private MeshRenderer playerMeshRenderer;

    void Start()
    {

    }

    private void Awake()
    {
        playerMeshRenderer = GameManager.instance.player.GetComponentInChildren<MeshRenderer>();
    }

    void Update()
    {
        
    }

    public void Show()
    {
        gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        GameManager.instance.Pause();
        GameManager.instance.player.GetComponentInChildren<MeshRenderer>().enabled = false;
        UIManager.instance.HideHUD();

    }

    public void Hide()
    {
        gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void RespawnButtonClicked()
    {
        GameManager.instance.player.GetComponentInChildren<MeshRenderer>().enabled = true;
        GameManager.instance.Resume();
        if (LevelManager.instance.currentCheckpoint != null)
        {
            GameManager.instance.player.Respawn(LevelManager.instance.currentCheckpoint.transform.position);
        }
        else
        {
            GameManager.instance.player.Respawn(LevelManager.instance.originalSpawnTransform.position);
        }
        UIManager.instance.HideDeathMessage();
        UIManager.instance.ShowHUD();
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene(0);
    }

}
