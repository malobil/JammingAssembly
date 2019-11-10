using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Script_UIManager : MonoBehaviour
{
    public static Script_UIManager Instance { get; private set; }

    public List<TextMeshProUGUI> playersGoldText;

    public GameObject endgameMenu;
    public GameObject VictoryPlayer1;
    public GameObject DeafeatPlayer1;
    public GameObject VictoryPlayer2;
    public GameObject DeafeatPlayer2;

    public TextMeshProUGUI timerMeshPro;
    public TextMeshProUGUI scoreMeshPlayer1;
    public TextMeshProUGUI scoreMeshPlayer2;

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

    public void UpdateTimer(float time)
    {
        timerMeshPro.text = time.ToString("F0");
    }

    public void ShowFinish()
    {
        endgameMenu.SetActive(true);
    }

    public void Retry()
    {
        SceneManager.LoadScene("Scene_Game");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Scene_MainMenu");
    }
}
