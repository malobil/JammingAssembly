using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class Script_UIManager : MonoBehaviour
{
    public static Script_UIManager Instance { get; private set; }

    public List<TextMeshProUGUI> playersGoldText;

    public GameObject endgameMenu;
    public List<GameObject> endPanel;
    public List<TextMeshProUGUI> scoreText;
    public Sprite winSprite;

    public TextMeshProUGUI timerMeshPro;

    public GameObject defaultSelection;

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

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
        {

            CatchMouseClicks(defaultSelection);

        }

    }

    public void CatchMouseClicks(GameObject setSelection)
    {

        EventSystem.current.SetSelectedGameObject(setSelection);

    }

    public void UpdateGoldText(int playerIdx, int goldValue)
    {
        playersGoldText[playerIdx].text = goldValue.ToString();
    }

    public void UpdateTimer(float time)
    {
        timerMeshPro.text = time.ToString("F0");
    }

    public void ShowFinish(int winnerPlayerIdx, List<int> scores)
    {
        for(int i = 0; i < endPanel.Count; i++)
        {
            if(winnerPlayerIdx == i)
            {
                endPanel[i].GetComponent<Image>().sprite = winSprite;
            }
        }

        for (int i = 0; i < scoreText.Count; i++)
        {
            scoreText[i].text = scores[i].ToString() + "m²" ;
        }

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
