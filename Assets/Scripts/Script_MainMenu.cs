using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Script_MainMenu : MonoBehaviour
{
    [Header("SceneManager")]

    public string s_game_scene;
	public GameObject defaultSelection;

    public void Awake()
    {
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;

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



    public void Play()
    {
        SceneManager.LoadScene(s_game_scene);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
