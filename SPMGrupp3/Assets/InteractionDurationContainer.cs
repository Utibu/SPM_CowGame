using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionDurationContainer : MonoBehaviour
{

    private Image durationMeter;
    private float interactionDuration;
    private BasicTimer duration;

    void Start()
    {
        durationMeter = GetComponent<Image>();
        durationMeter.fillAmount = 0;
        duration = new BasicTimer(interactionDuration);
    }

    void Update()
    {
        if(gameObject.activeSelf == true)
        {
            durationMeter.fillAmount = duration.GetPercentage();
            if (duration.IsCompleted(Time.deltaTime, true, true))
            {
                Hide();
            }
        }

    }

    public void Show(float duration)
    {
        gameObject.SetActive(true);
        interactionDuration = duration;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        duration.Reset();
    }
}
