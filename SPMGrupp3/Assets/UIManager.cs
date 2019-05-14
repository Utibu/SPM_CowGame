using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;

    [SerializeField] private Image dashCooldownImage;


    [SerializeField] private Text smallMessageTitle;
    [SerializeField] private Text smallMessageDescription;
    [SerializeField] private Image smallMessageImage;
    [SerializeField] private Image smallMessageContainer;

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
        smallMessageContainer.gameObject.SetActive(false);
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

    public void ShowSmallMessage(string title, string desc, Sprite sprite)
    {
        smallMessageContainer.gameObject.SetActive(true);
        smallMessageTitle.text = title;
        smallMessageDescription.text = desc;
        smallMessageImage.sprite = sprite;
    }

    public void HideMessage()
    {
        smallMessageContainer.gameObject.SetActive(false);
    }
}
