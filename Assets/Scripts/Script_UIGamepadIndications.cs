using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Script_UIGamepadIndications : MonoBehaviour
{
    public int playerIdx;
    public Sprite ps4Sprite;
    public Sprite xboxSprite;

    private Image imageComp;

    private void Start()
    {
        imageComp = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Script_GameManager.Instance.GetPlayerGamePad(playerIdx) == Gamepad.PS4)
        {
            imageComp.sprite = ps4Sprite;
        }
        else if(Script_GameManager.Instance.GetPlayerGamePad(playerIdx) == Gamepad.XBOX)
        {
            imageComp.sprite = xboxSprite;
        }
    }
}
