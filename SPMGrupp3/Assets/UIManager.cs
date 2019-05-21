using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;

    [SerializeField] private Image dashCooldownImage;
    private Color dashCooldownImageColor;
    [SerializeField] private GameObject interactionIndicator;
    [SerializeField] private Image menu;

    [SerializeField] private SmallMessageContainer smallMessageContainer;
    [SerializeField] private BigMessageContainer bigMessageContainer;
    [SerializeField] private Image speedLines;

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
        dashCooldownImageColor = dashCooldownImage.color;
    }

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

    public void DeactivateDashBar()
    {
        dashCooldownImage.color = Color.gray;
    }

    public void ActivateDashBar()
    {
        dashCooldownImage.color = dashCooldownImageColor;
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

    public void HideMessages()
    {
        smallMessageContainer.gameObject.SetActive(false);
        bigMessageContainer.gameObject.SetActive(false);
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
