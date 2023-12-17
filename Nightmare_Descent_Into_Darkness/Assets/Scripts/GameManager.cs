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

   /* private void Update()
    {
      *//*  if (PassedLevels.Count == 3)
        {
            Debug.Log("ALL three levelsd passed");
        }*//*
    }*/

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
        //TODO: if door not in dictionary, directly return
        if (!LevelMatch.ContainsKey(door))
        {
            Debug.Log("Door not found");
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

    public void LockMove()
    {
        firstPersonController.GetComponent<FirstPersonController>().CanMove = false;
    }

    public void ReleaseMove()
    {
        firstPersonController.GetComponent<FirstPersonController>().CanMove = true;
    }
    public void LockView()
    {
        firstPersonController.GetComponent<FirstPersonController>().CanRotate = false;
    }

    public void ReleaseView()
    {
        firstPersonController.GetComponent<FirstPersonController>().CanRotate = true;
    }


}
