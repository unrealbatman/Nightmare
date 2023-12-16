using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class BossCutsceneHandler : MonoBehaviour
{

    public Camera mainCam;
    public GameObject cineMachineCam;
    public PlayableDirector director;
    public Canvas cutsceneCanvas;
    public GameObject LightHouse;
    
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.PassedLevels.Count == 1)
        {
            
            SwitchCam();
            if(director != null)
            {
                director.gameObject.SetActive(true);
                director.Play();
                Cursor.lockState = CursorLockMode.None;

            }
            if(director.time == 20.0)
            {
                SwitchCam();
                Cursor.lockState = CursorLockMode.Locked;


            }
        }
    }

    public void SwitchCam()
    {

        mainCam.gameObject.SetActive(!mainCam.gameObject.activeInHierarchy);
       // cineMachineCam.gameObject.SetActive(!cineMachineCam.activeInHierarchy);
        cutsceneCanvas.gameObject.SetActive(!cutsceneCanvas.gameObject.activeInHierarchy);
    }
}
