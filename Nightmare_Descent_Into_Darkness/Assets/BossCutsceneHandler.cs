using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class BossCutsceneHandler : MonoBehaviour
{

    public Camera mainCam;
    public GameObject cineMachineCam;
    
    // Start is called before the first frame update
    void Start()
    {
        PlayableDirector timeline = GetComponent<PlayableDirector>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchCam()
    {
        if()
        mainCam.gameObject.SetActive(true);
        cineMachineCam.gameObject.SetActive(false);

    }
}
