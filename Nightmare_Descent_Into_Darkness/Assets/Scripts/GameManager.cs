using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This class is for containing global controls like game process variables and Scene controls
/// </summary>
public class GameManager : MonoBehaviour{

    struct SavePoint
    {
        public Vector3 position;
            
    };
    public const string MainScene = "Main";

    [SerializeField]
    public GameObject firstPersonController;
    public static GameManager Instance { get; private set; }

    private SavePoint savePoint = new SavePoint();

    public List<string> Levels = new List<string>();
    public List<string> PassedLevels = new List<string>();

    public Dictionary<GameObject, string> LevelMatch = new Dictionary<GameObject, string>();

    //record the currentLevel
    public string currentLevel = MainScene;

    public GameObject notePanel;

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

    public void Start()
    {
        if(PassedLevels.Count < 1)
        {
            return;
        }
        else if(PassedLevels[0] == "Level3" || PassedLevels[1] == "Level3" || PassedLevels[2] == "Level3")
        {
            DisplayNote();
        }
    }

    public void DisplayNote()
    {
        notePanel.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
    }

    public void CloseNote()
    {
        notePanel.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
    }


    // for visiting scenes in order
    public void LoadLevel()
    {
        savePoint.position = firstPersonController.transform.position;
        SceneManager.LoadScene(Levels[0]);
    }

    // for test
    public void LoadLevel(string levelName)
    {
        savePoint.position = firstPersonController.transform.position;
        SceneManager.LoadScene(levelName);
    }

    public void LoadLevel(GameObject door)
    {
        if(!LevelMatch.ContainsKey(door))
        {
            Debug.LogError("Door not found in LevelMatch");
            return;
        } 
        savePoint.position = firstPersonController.transform.position;
        SceneManager.LoadScene(LevelMatch[door]);
        currentLevel = LevelMatch[door];
    }

    public void BackToMain()
    {
        SceneManager.LoadScene(MainScene);
        currentLevel = MainScene;
    }



    public Vector3  GetSavePoint()
    {
        return savePoint.position;
    }

    public void LevelFinish(string levelName)
    {

        PassedLevels.Add(levelName);

        // TODO: can trigger some level end effect here 
    }



        
}
