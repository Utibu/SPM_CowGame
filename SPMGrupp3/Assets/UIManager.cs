using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;

    //[SerializeField] private Image dashCooldownImage;
    [SerializeField] private Image healthBarImage;
    [SerializeField] private Image healthBarBackgroundImage;
    [SerializeField] private Text coinCountText;
    private Color dashCooldownImageColor;
    [SerializeField] private GameObject interactionIndicator;
    [SerializeField] private Image menu;

    [SerializeField] private SmallMessageContainer smallMessageContainer;
    [SerializeField] private BigMessageContainer bigMessageContainer;
    [SerializeField] private ButtonMessageContainer deathMessageContainer;
    [SerializeField] private VictoryMessageContainer victoryMessageContainer;
    [SerializeField] private InteractionDurationContainer interactionDurationContainer;

    // new ram mätare
    [SerializeField] private Image RamActiveImage;
    [SerializeField] private Image RamCooldownImage;

    [SerializeField] private Image speedLines;
    [SerializeField] private Image saveImage;

    [SerializeField] private Image loadingImage;
    public Image LoadingImage { get { return loadingImage; } private set { loadingImage = value; } }
    [SerializeField] private Image loadingBar;
    public Image LoadingBar { get { return loadingBar; } private set { loadingBar = value; } }
    public Text mouseDebug; 

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        HideMessages();
        HideInteractionIndicator();
        HideMenu();
        HideSpeedlines();
        HideSavingIcon();
        loadingImage.gameObject.SetActive(false);
        //dashCooldownImageColor = dashCooldownImage.color;
    }
    /*
    public void SetDashFillAmount(float val)
    {
        if (dashCooldownImage != null)
        {
            dashCooldownImage.fillAmount = val;

        }
    }

    public void SetDashFillAmountAdd(float val)
    {
        SetDashFillAmount(dashCooldownImage.fillAmount + val);
    }
    */
    // nya ram mätarens metoder
    public void UpdateRamCooldown(float value)
    {
        if (RamCooldownImage != null)
        {
            RamCooldownImage.fillAmount = value;
        }
    }

    public void SetRamReady(bool isReady)
    {
        if (isReady)
        {
            RamActiveImage.color = Color.white;

        }
        else if (!isReady)
        {
            RamActiveImage.color = Color.grey;
        }
    }

    public void SetRamImage(bool isDashing)
    {
        if (isDashing)
        {
            RamActiveImage.color = Color.red;
            //RamActiveImage.CrossFadeColor(Color.red, 0.5f, false, true);
            // gör ngt snyggt
        }
        
        else
        {
            RamActiveImage.color = Color.grey;
        }
        
    }

    /*
    public void DeactivateDashBar()
    {
        dashCooldownImage.color = Color.gray;
    }

    public void ActivateDashBar()
    {
        dashCooldownImage.color = dashCooldownImageColor;
    }
    */

    public void ShowSavingIcon()
    {
        saveImage.gameObject.SetActive(true);
        Invoke("HideSavingIcon", 2f);
    }

    public void HideSavingIcon()
    {
        saveImage.gameObject.SetActive(false);
    }

    public void ShowSmallMessage(string title, string desc, Sprite sprite)
    {
        HideMessages();
        smallMessageContainer.Show(title, desc, sprite);
    }

    public void ShowBigMessage(string title, string leftDesc, Sprite leftSprite, string rightDesc, Sprite rightSprite)
    {
        HideMessages();
        bigMessageContainer.Show(title, leftDesc, leftSprite, rightDesc, rightSprite);
    }

    public void ShowDeathMessage()
    {
        HideMessages();
        deathMessageContainer.Show();
    }

    public void HideDeathMessage()
    {
        deathMessageContainer.gameObject.SetActive(false);
    }

    public void ShowVictoryMessage()
    {
        victoryMessageContainer.Show();
    }

    public void ShowInteractionMeter(float duration)
    {
        interactionDurationContainer.Show(duration);
    }

    public void HideMessages()
    {
        smallMessageContainer.gameObject.SetActive(false);
        bigMessageContainer.gameObject.SetActive(false);
        deathMessageContainer.gameObject.SetActive(false);
    }

    public void ShowHUD()
    {
        healthBarImage.gameObject.SetActive(true);
        healthBarBackgroundImage.gameObject.SetActive(true);
        //dashCooldownImage.gameObject.SetActive(true);
        coinCountText.gameObject.SetActive(true);
    }

    public void HideHUD()
    {
        healthBarImage.gameObject.SetActive(false);
        healthBarBackgroundImage.gameObject.SetActive(false);
        //dashCooldownImage.gameObject.SetActive(false);
        coinCountText.gameObject.SetActive(false);
    }

    public void ShowInteractionIndicator()
    {
        interactionIndicator.SetActive(true);
    }

    public void HideInteractionIndicator()
    {
        interactionIndicator.SetActive(false);
    }

    public void ShowMenu()
    {
        menu.gameObject.SetActive(true);
    }

    public void HideMenu()
    {
        menu.gameObject.SetActive(false);
    }

    public void MenuQuitButtonOnClick()
    {
        GameManager.instance.LoadMenu();
    }

    public void MenuResumeButtonOnClick()
    {
        GameManager.instance.Resume();
        HideMenu();
    }

    public void ShowSpeedlines()
    {
        speedLines.gameObject.SetActive(true);
    }

    public void HideSpeedlines()
    {
        speedLines.gameObject.SetActive(false);
    }
}
