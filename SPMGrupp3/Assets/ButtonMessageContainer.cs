using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonMessageContainer : MonoBehaviour
{
    //[SerializeField] private PlayerStateMachine player;
    //private PlayerStateMachine stateMachine;
    private MeshRenderer playerMeshRenderer;
    [SerializeField] private Image container;

    void Start()
    {

    }

    private void Awake()
    {
        playerMeshRenderer = GameManager.instance.player.GetComponentInChildren<MeshRenderer>();
    }

    void Update()
    {
        if(container.gameObject.activeSelf == true && Input.GetKeyDown(KeyCode.F))
        {
            RespawnButtonClicked();
        }
        else if(container.gameObject.activeSelf == true && Input.GetKeyDown(KeyCode.Escape))
        {
            MainMenuButton();
        }
    }

    public virtual void Show()
    {
        //gameObject.SetActive(true);
        

    }

    public void Hide()
    {
        //gameObject.SetActive(false);
        
    }

    public void RespawnButtonClicked()
    {
        ResumeGame(true);
        /*if (LevelManager.instance.currentCheckpoint != null)
        {
            GameManager.instance.player.Respawn(LevelManager.instance.currentCheckpoint.transform.position);
        }
        else
        {
            GameManager.instance.player.Respawn(LevelManager.instance.originalSpawnTransform.position);
        }*/
        GameManager.instance.SaveManager.Load();
        
    }

    public void RestartLevelButtonClicked()
    {
        
        GameManager.instance.coinCount -= LevelManager.instance.pickedCoins;
        GameManager.instance.totalCoinCount -= LevelManager.instance.pickedCoins;
        GameManager.instance.LoadScene(SceneManager.GetActiveScene().buildIndex);
        ResumeGame();
        //GameManager.instance.player.Respawn(LevelManager.instance.originalSpawnTransform.position);
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene(0);
    }

    private void ResumeGame(bool shouldDoResumeAction = false)
    {
        UIManager.instance.ResumeGame(shouldDoResumeAction);
    }

}
