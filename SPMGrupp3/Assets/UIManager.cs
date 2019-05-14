using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;

    [SerializeField] private Image dashCooldownImage;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
        {
            Destroy(gameObject);
        }
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
}
