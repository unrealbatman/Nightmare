using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TestVisibility : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //OnBecameInvisible does get called even when you exit play mode. This is as expected because at this point, the Renderers are no longer being rendered by the camera in play mode.

    //Further, OnBecameInvisible & OnBecameVisible are also called by the scene view camera.Therefore, if your renderer is no longer visible to the play mode camera, it might still be visible to the scene mode camera
    void OnBecameInvisible()
    {
        Debug.Log("I`m gone :(");
    }
    void OnBecameVisible()
    {
        Debug.Log("Hi, I`m back!");
    }
}
