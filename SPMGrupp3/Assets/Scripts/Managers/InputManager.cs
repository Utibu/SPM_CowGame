﻿//Main Author: Niklas Almqvist
//Secondary Author: Joakim Ljung
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager
{
    public bool EventKeyDown()
    {
        if(Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Joystick1Button2))
        {
            return true;
        }
        return false;
    }

    public bool JumpKeyDown()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Joystick1Button0))
        {
            return true;
        }
        return false;
    }

    public bool DashKey()
    {
        if((Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift)) || (Input.GetAxis("Right Trigger") > 0 && Input.GetAxisRaw("Vertical") >= 0.60f))
        {
            return true;
        }
        return false;
    }

    public bool SideDashKey()
    {
        if(Input.GetKeyDown(KeyCode.LeftControl) || Input.GetAxis("Left Trigger") > 0)
        {
            return true;
        }
        return false;
    }

    public bool ForwardKey()
    {
        if(Input.GetKey(KeyCode.W) && Input.GetAxisRaw("Vertical") == 1)
        {
            return true;
        }
        return false;
    }

    public bool CancelKeyDown()
    {
        if(Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Joystick1Button1))
        {
            return true;
        }
        return false;
    }

    public bool MenuButtonDown()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Joystick1Button7))
        {
            return true;
        }
        return false;
    }

    
}
