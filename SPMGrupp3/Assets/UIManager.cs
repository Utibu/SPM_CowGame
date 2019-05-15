using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;

    [SerializeField] private Image dashCooldownImage;

    [SerializeField] private SmallMessageContainer smallMessageContainer;
    [SerializeField] private BigMessageContainer bigMessageContainer;

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
}
