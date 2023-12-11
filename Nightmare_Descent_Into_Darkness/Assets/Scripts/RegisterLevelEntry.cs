using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RegisterLevelEntry : MonoBehaviour
{
    [SerializeField]
    private int levelIndex = 0;
    private void OnEnable()
    {
        SceneManager.sceneLoaded += onSceneLoaded;

    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= onSceneLoaded;
    }

    private void onSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string levelName = GameManager.Instance.Levels[levelIndex];
        if (GameManager.Instance.PassedLevels.Contains(levelName))
        {
            this.gameObject.layer = 0;
            return;
        }
        else
        {
            GameManager.Instance.LevelMatch.Add(gameObject, levelName);
        }
        
       
    }
}
