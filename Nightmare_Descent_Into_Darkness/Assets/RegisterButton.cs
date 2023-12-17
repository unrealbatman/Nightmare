using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RegisterButton : MonoBehaviour
{
    private void OnEnable()
    {
        RegisterGameManager();
    }

    private void OnDisable()
    {

    }

    private void RegisterGameManager()
    {
        Button bt= GetComponent<Button>();
        Debug.Log("skip button");
        bt.onClick.AddListener(()=> GameManager.Instance.LoadLevel("GameOverMenu"));
            
    }

}
