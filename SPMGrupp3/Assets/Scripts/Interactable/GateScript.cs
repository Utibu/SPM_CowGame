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

    private float newEulerLeftY;
    private float newEulerRightY;
    private float currentEulerLeftY;
    private float currentEulerRightY;

    private bool isCurrentlyAnimating = false;
    public bool IsOpened = false;
    private BasicTimer animationTimer;

    private void Start()
    {
        newEulerLeftY = Helper.GetCorrectAngle(transform.eulerAngles.y + (-90f));
        newEulerRightY = Helper.GetCorrectAngle(transform.eulerAngles.y + (90f));
        currentEulerLeftY = Helper.GetCorrectAngle(leftGate.transform.eulerAngles.y);
        currentEulerRightY = Helper.GetCorrectAngle(rightGate.transform.eulerAngles.y);
        GameManager.instance.SaveManager.Gates.Add(this);
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
            if (GameManager.instance.player.IsDashing)
            {
                GameManager.instance.player.velocity *= -1;
                GameManager.instance.player.ShakeCamera();
            }
        }
    }

    private void Open()
    {
        isCurrentlyAnimating = true;
        IsOpened = true;

        if(GameManager.instance.player.IsDashing)
        {
            animationTimer = new BasicTimer(0.2f);
        } else
        {
            animationTimer = new BasicTimer(2f);
        }
        GetComponent<Collider>().enabled = false;

    }

    private void Update()
    {
        if(isCurrentlyAnimating && animationTimer != null)
        {
            if(animationTimer.IsCompleted(Time.deltaTime, false, true) == false)
            {
                currentEulerLeftY = Mathf.Lerp(currentEulerLeftY, newEulerLeftY, animationTimer.GetPercentage());
                currentEulerRightY = Mathf.Lerp(currentEulerRightY, newEulerRightY, animationTimer.GetPercentage());
                leftGate.transform.eulerAngles = new Vector3(0f, currentEulerLeftY, 0f);
                rightGate.transform.eulerAngles = new Vector3(0f, currentEulerRightY, 0f);
            }
        }
    }


}
