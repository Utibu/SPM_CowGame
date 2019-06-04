//Main Author: Niklas Almqvist
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BigMessageContainer : MonoBehaviour
{
    [SerializeField] private Text messageTitle;

    [SerializeField] private Text leftMessageDescription;
    [SerializeField] private Image leftMessageImage;

    [SerializeField] private Text rightMessageDescription;
    [SerializeField] private Image rightMessageImage;

    public void Show(string title, string leftDesc, Sprite leftSprite, string rightDesc, Sprite rightSprite)
    {
        gameObject.SetActive(true);
        messageTitle.text = title;
        leftMessageDescription.text = leftDesc;
        leftMessageImage.sprite = leftSprite;
        rightMessageDescription.text = rightDesc;
        rightMessageImage.sprite = rightSprite;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
