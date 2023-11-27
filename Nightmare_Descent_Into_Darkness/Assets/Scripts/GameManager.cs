using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;





/// <summary>
/// This class is for containing global controls like game process variables and Scene controls
/// </summary>
public class GameManager : MonoBehaviour
{

    public const string MainScene = "Main";

    [SerializeField]
    private FirstPersonController firstPersonController;
    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        // public static instance exposed
        Instance = this;

        // keep it across all scenes
        DontDestroyOnLoad(gameObject);
    }

    public List<string> RemainLevels = new List<string>();
    public List<string> PassedLevels= new List<string>();

    // for visiting scenes in order
    public void LoadLevel()
    {
        SceneManager.LoadScene(RemainLevels[0]);
    }

    // for test
    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public void BackToMain()
    {
        SceneManager.LoadScene(MainScene);
    }

    public void LockMove()
    {
        firstPersonController.CanMove = false;
    }

    public void ReleaseMove()
    {
        firstPersonController.CanMove = true;
    }
}
