using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Register : MonoBehaviour
{   
    private Vector3 positionOffset = Vector3.zero;

    
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

        positionOffset = restorePosition - transform.position;
        gameObject.GetComponent<CharacterController>().enabled = false;
        transform.position += positionOffset;
        gameObject.GetComponent<CharacterController>().enabled = true;
        //Debug.Log("restore:" + restorePosition);
        //Debug.Log("offset:" + positionOffset);
        //Debug.Log(transform.position);
    }

}
