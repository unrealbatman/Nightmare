using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadOnActivation : MonoBehaviour
{
    [SerializeField] int sceneIndex;
    private void OnEnable()
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
