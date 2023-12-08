using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Register : MonoBehaviour
{   
    private Vector3 positionOffset = Vector3.zero;

    // register delegates of sceneloaded
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
        if (GameManager.Instance.firstPersonController == null)
        {
            GameManager.Instance.firstPersonController = gameObject;
        }
        Vector3 restorePosition = GameManager.Instance.GetSavePoint();

        if(restorePosition == Vector3.zero)
        {
            return;
        }

        // reference: https://stackoverflow.com/questions/66378459/unity-set-player-position-after-loading-scene
        positionOffset = restorePosition - transform.position;
        gameObject.GetComponent<CharacterController>().enabled = false;
        transform.position += positionOffset;
        gameObject.GetComponent<CharacterController>().enabled = true;
        Debug.Log("restore:" + restorePosition);
        Debug.Log("offset:" + positionOffset);
        Debug.Log(transform.position);
    }

}
