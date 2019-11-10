using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Script_UIManager : MonoBehaviour
{
    public static Script_UIManager Instance { get; private set; }

    public List<TextMeshProUGUI> playersGoldText; 

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void UpdateGoldText(int playerIdx, int goldValue)
    {
        playersGoldText[playerIdx].text = goldValue.ToString();
    }
}
