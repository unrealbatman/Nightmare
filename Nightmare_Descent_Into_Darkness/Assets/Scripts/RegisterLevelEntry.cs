using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.SceneManagement;

public class RegisterLevelEntry : MonoBehaviour
{
    [SerializeField]
    private int levelIndex = 0;

    [SerializeField]
    private List<GameObject> Fires;

    [SerializeField]
    private List<GameObject> Lights;
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
        if (scene != SceneManager.GetSceneByName(GameManager.MainScene))
        {
            return;
        }
        string levelName = GameManager.Instance.Levels[levelIndex];
        if (GameManager.Instance.PassedLevels.Contains(levelName))
        {
            //disable level entry
            this.gameObject.layer = 0;
            ExtinguishFire();
            DisableLight();

            CheckTriggerDoor ch = GetComponent<CheckTriggerDoor>();
            ch.CloseText();
            Destroy(ch);
            return;
        }
        else
        {
            GameManager.Instance.LevelMatch.Add(gameObject, levelName);
            Debug.Log(levelName + "registered");
        }
        if (GameManager.Instance.PassedLevels.Count == 2)
        {
            Debug.Log("Passed levels: " + GameManager.Instance.PassedLevels.Count);

        }

    }
    private void ExtinguishFire()
    {
        foreach (GameObject f in Fires)
        {
            f.SetActive(false);
            //f.GetComponent<ParticleSystem>().Stop();
            //f.GetComponent<ParticleSystem>().Clear();
        }
    }

    private void LightUpFire()
    {
        foreach (GameObject f in Fires)
        {
            f.SetActive(true);
            f.GetComponent<ParticleSystem>().Play();
        }
    }
    
    private void DisableLight()
    {
        foreach (GameObject l in Lights)
        {
            l.SetActive(false) ;


        }
    }

}
