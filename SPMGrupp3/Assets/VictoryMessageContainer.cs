using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VictoryMessageContainer : ButtonMessageContainer
{
    public Text coinText;

    public override void Show()
    {
        base.Show();
        coinText.text = "Total coins collected: " + GameManager.instance.totalCoinCount;
    }

}
