using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;

    [SerializeField] private Image dashCooldownImage;
    [SerializeField] private Text messageText;
    [SerializeField] private Image messageContainer;

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
        messageContainer.gameObject.SetActive(false);
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

    public void ShowMessage(string textToShow)
    {
        messageContainer.gameObject.SetActive(true);
        messageText.text = textToShow;
    }

    public void HideMessage()
    {
        messageContainer.gameObject.SetActive(false);
    }
}
