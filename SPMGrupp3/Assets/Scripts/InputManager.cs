using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager
{
    public bool EventKeyPressed()
    {
        if(Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Joystick1Button0))
        {
            return true;
        }
        return false;
    }
}
