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
    [SerializeField] private Image loadingImage;
    [SerializeField] private Image loadingBar;
    [SerializeField] private List<Button> buttons;
    private InputManager inputManager = new InputManager();
    private BasicTimer controllerTimer;
    int currentlyChosenMenuItem;
    private Color originalColor;
    [SerializeField] private Color selectedColor;

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
        loadingImage.gameObject.SetActive(false);
        controllerTimer = new BasicTimer(0.2f);

        //https://www.sitepoint.com/saving-and-loading-player-game-data-in-unity/
        localSaveModel = Helper.GetSaveFile();
        if(localSaveModel != null && localSaveModel.OnLevel <= 2)
        {
            continueButton.gameObject.SetActive(true);
            buttons.Insert(0, continueButton);
            GameInformation.ShouldContinue = true;
        }
        originalColor = buttons[currentlyChosenMenuItem].GetComponent<Image>().color;
        currentlyChosenMenuItem = -1;
        //buttons[currentlyChosenMenuItem].GetComponent<Image>().color = selectedColor;
    }

    // Update is called once per frame
    void Update()
    {
        if(controllerTimer.IsCompleted(Time.deltaTime, false, true) && Input.GetAxisRaw("Vertical") != 0) {
            controllerTimer.Reset();
            float vertical = Input.GetAxisRaw("Vertical");
            if(currentlyChosenMenuItem >= 0)
            {
                buttons[currentlyChosenMenuItem].GetComponent<Image>().color = originalColor;
            }
            
            if (vertical > 0)
            {
                currentlyChosenMenuItem = (currentlyChosenMenuItem - 1 + buttons.Count) % buttons.Count;
            } else
            {
                currentlyChosenMenuItem = (currentlyChosenMenuItem + 1) % buttons.Count;
            }
            buttons[currentlyChosenMenuItem].GetComponent<Image>().color = selectedColor;
            Debug.Log("CURRENT INDEX: " + currentlyChosenMenuItem);
        }

        if(inputManager.JumpKeyDown())
        {
            buttons[currentlyChosenMenuItem].onClick.Invoke();
        }
    }

    public void ContinueButtonClicked()
    {
        LoadScene(localSaveModel.OnLevel);
        Debug.Log("SAVEMODEL ONLEVEL: " + localSaveModel.OnLevel);
    }

    public void PlayButtonClicked()
    {
        LoadScene(1);
        GameInformation.ShouldContinue = false;
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
        loadingImage.gameObject.SetActive(true);
        loadingBar.fillAmount = 0f;

        AsyncOperation loadingPlayer = SceneManager.LoadSceneAsync(3, LoadSceneMode.Additive);
        loadingPlayer.allowSceneActivation = false;
        while (!loadingPlayer.isDone)
        {
            loadingBar.fillAmount = Mathf.Clamp(loadingPlayer.progress, 0f, 0.5f);

            if (loadingPlayer.progress >= 0.9f)
            {
                loadingPlayer.allowSceneActivation = true;
                
            }

            yield return null;
        }

        AsyncOperation loading = SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);
        loading.allowSceneActivation = false;
        while (!loading.isDone)
        {
            loadingBar.fillAmount = Mathf.Clamp(loading.progress, 0.5f, 1f);

            if (loading.progress >= 0.9f)
            {
                loading.allowSceneActivation = true;
                loadingPlayer.allowSceneActivation = true;
            }

            yield return null;
        }
        Scene scene = SceneManager.GetSceneByBuildIndex(index);
        SceneManager.SetActiveScene(scene);
        GameInformation.OnLevel = index;
        foreach (GameObject go in scene.GetRootGameObjects())
        {
            if (go.CompareTag("LevelManager"))
            {
                go.GetComponent<LevelManager>().LoadGame();
            }
        }
        //Debug.LogWarning("FDF");
        AsyncOperation unloadCurrent = SceneManager.UnloadSceneAsync(0);
        yield return unloadCurrent;
        
        isLoadingScene = false;
    }

    

    public void QuitButtonClicked()
    {
        Application.Quit();
    }
}
