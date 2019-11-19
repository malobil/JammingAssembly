using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_EventSystems : MonoBehaviour
{
    public GameObject eventSystemPs4;
    public GameObject eventSystemXbox;
    private Gamepad gamepadType;


    // Update is called once per frame
    void Update()
    {
        string[] gamepads = Input.GetJoystickNames();

        for (int i = 0; i < gamepads.Length; i++)
        {
            if (i == 0)
            {
                if (gamepads[i].Length == 19)
                {
                    gamepadType = Gamepad.PS4;
                }
                else if (gamepads[i].Length == 33)
                {
                    gamepadType = Gamepad.XBOX;
                }
            }
        }

        if(gamepadType == Gamepad.PS4)
        {
            eventSystemPs4.SetActive(true);
            eventSystemXbox.SetActive(false);
        }
        else if(gamepadType == Gamepad.XBOX)
        {
            eventSystemPs4.SetActive(false);
            eventSystemXbox.SetActive(true);
        }
    }
}
