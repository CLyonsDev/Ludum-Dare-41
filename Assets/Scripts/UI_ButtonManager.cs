using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_ButtonManager : MonoBehaviour {

    public GameObject controlsGO;
    public GameObject levelSelectGO;
	
	public void StartGame(int level)
    {
        SceneManager.LoadScene(level);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ShowControls()
    {

    }

    public void ShowCredits()
    {

    }

    public void ShowLevelSelect()
    {

    }
}
