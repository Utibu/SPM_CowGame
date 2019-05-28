using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class MenuManager : MonoBehaviour
{

    public int playIndex;
    private bool isLoadingScene = false;
    private SaveModel localSaveModel;
    [SerializeField] private Button continueButton;

    private void Awake()
    {
        continueButton.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        GameInformation.ShouldContinue = false;

        //https://www.sitepoint.com/saving-and-loading-player-game-data-in-unity/
        localSaveModel = Helper.GetSaveFile();
        if(localSaveModel != null && localSaveModel.OnLevel <= 2)
        {
            continueButton.gameObject.SetActive(true);
            GameInformation.ShouldContinue = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ContinueButtonClicked()
    {
        LoadScene(localSaveModel.OnLevel);
        Debug.Log("SAVEMODEL ONLEVEL: " + localSaveModel.OnLevel);
    }

    public void PlayButtonClicked()
    {
        LoadScene(1);
    }

    public void LoadScene(int index)
    {
        /*SceneManager.UnloadSceneAsync(currentSceneIndex);
        SceneManager.LoadScene(index, LoadSceneMode.Additive);*/
        if (!isLoadingScene)
        {

            

            IEnumerator coroutine = LoadSceneRoutine(index);

            //SceneManager.LoadScene(3);
            isLoadingScene = true;

            StartCoroutine(coroutine);

            

            //SceneManager.UnloadSceneAsync(0);

        }

    }
    
    IEnumerator LoadSceneRoutine(int index)
    {
        var loadingPlayer = SceneManager.LoadSceneAsync(3, LoadSceneMode.Additive);
        yield return loadingPlayer;
        var loading = SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);
        yield return loading;
        var scene = SceneManager.GetSceneByBuildIndex(index);
        SceneManager.SetActiveScene(scene);
        //Debug.LogWarning("FDF");
        var unloadCurrent = SceneManager.UnloadSceneAsync(0);
        yield return unloadCurrent;
        
        isLoadingScene = false;
    }

    

    public void QuitButtonClicked()
    {
        Application.Quit();
    }
}
