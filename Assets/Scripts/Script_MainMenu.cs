using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Script_MainMenu : MonoBehaviour
{
    [Header("SceneManager")]

    public string s_game_scene;

    public void Awake()
    {

    }

    public void Play()
    {
        SceneManager.LoadScene(s_game_scene);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
