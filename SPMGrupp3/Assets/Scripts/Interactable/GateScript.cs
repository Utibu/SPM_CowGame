using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateScript : Collidable
{

    public GameObject gateKey;
    [SerializeField] private GameObject leftGate;
    [SerializeField] private GameObject rightGate;
    private Vector3 originalEulerLeft;
    private Vector3 originalEulerRight;
    private Vector3 newEulerLeft;
    private Vector3 newEulerRight;

    private bool isCurrentlyAnimating = false;
    private bool isOpened = false;
    private BasicTimer animationTimer;

    private void Start()
    {
        newEulerLeft = new Vector3(0f, transform.eulerAngles.y + (-90f), 0f);
        newEulerRight = new Vector3(0f, transform.eulerAngles.y + (90f), 0f);
    }

    public override void OnPlayerCollideEnter(Collider hitCollider, out bool skipCollision)
    {

        if (LevelManager.instance.hasGateKey)
        {
            skipCollision = true;
            GameManager.instance.player.ShakeCamera();
            Open();
        }
        else
        {
            skipCollision = false;
            if (GameManager.instance.player.GetCurrentState().GetType() == typeof(DashState))
            {
                GameManager.instance.player.velocity *= -1;
                GameManager.instance.player.ShakeCamera();
            }
        }
    }

    private void Open()
    {
        // do a nice gate open rotation or start anim or whatever
        isCurrentlyAnimating = true;
        isOpened = true;

        if(GameManager.instance.player.GetCurrentState().GetType() == typeof(DashState))
        {
            animationTimer = new BasicTimer(0.2f);
        } else
        {
            animationTimer = new BasicTimer(2f);
        }

        originalEulerLeft = transform.eulerAngles + leftGate.transform.eulerAngles;
        originalEulerRight = transform.eulerAngles + rightGate.transform.eulerAngles;
        GetComponent<Collider>().enabled = false;
        //Destroy(this.gameObject);

    }

    private void Update()
    {
        if(isCurrentlyAnimating && animationTimer != null)
        {
            if(animationTimer.IsCompleted(Time.deltaTime, false, true) == false)
            {
                leftGate.transform.eulerAngles = Vector3.Slerp(leftGate.transform.eulerAngles, newEulerLeft, animationTimer.GetPercentage());
                rightGate.transform.eulerAngles = Vector3.Slerp(rightGate.transform.eulerAngles, newEulerRight, animationTimer.GetPercentage());
                //leftGate.transform.eulerAngles = newEulerLeft;
                //rightGate.transform.eulerAngles = newEulerRight;
            }
        }
    }


}
