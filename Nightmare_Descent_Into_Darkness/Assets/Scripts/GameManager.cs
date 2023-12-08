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

        struct SavePoint
        {
            public Vector3 position;
            
        };
        public const string MainScene = "Main";

        [SerializeField]
        public GameObject firstPersonController;
        public static GameManager Instance { get; private set; }

        private SavePoint savePoint = new SavePoint();
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
            savePoint.position = firstPersonController.transform.position;
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
            //firstPersonController.transform.position = savePoint.position;
        }


        public void HandleFailure()
        {
            Debug.Log("Failed");
        }


        public Vector3  GetSavePoint()
        {
            return savePoint.position;
        }

        
    }
