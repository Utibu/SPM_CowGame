using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmallMessageContainer : MonoBehaviour
{
    [SerializeField] private Text messageTitle;
    [SerializeField] private Text messageDescription;
    [SerializeField] private Image messageImage;

    public void Show(string title, string desc, Sprite sprite)
    {
        gameObject.SetActive(true);
        messageTitle.text = title;
        messageDescription.text = desc;
        messageImage.sprite = sprite;
        messageImage.preserveAspect = true;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
