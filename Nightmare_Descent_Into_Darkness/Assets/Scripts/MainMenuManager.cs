using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject controlsPanel;

    public void StartGame()
    {
        SceneManager.LoadScene("IntroCutscene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenControls()
    {
        mainMenuPanel.SetActive(false);
        controlsPanel.SetActive(true);
    }

    public void Back()
    {
        mainMenuPanel.SetActive(true);
        controlsPanel.SetActive(false);
    }
}
