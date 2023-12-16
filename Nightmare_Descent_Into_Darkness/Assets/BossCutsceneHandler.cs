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
    public Collider sphereCol;

    bool cutscenePlayed = false;

    void Update()
    {
        if (cutscenePlayed==false && GameManager.Instance.PassedLevels.Count == 3)
        {

            StartCutscene();
        }
       
    }

    void StartCutscene()
    {
        mainCam.gameObject.SetActive(false);
        cutsceneCanvas.gameObject.SetActive(true);
        cineMachineCam.gameObject.SetActive(true);
        cutscenePlayed = true;

        if (director != null)
        {
            director.gameObject.SetActive(true);
            director.Play();
            Cursor.lockState = CursorLockMode.None;
            StartCoroutine(EndCutsceneAfterDuration(director.duration));
        }
    }

    IEnumerator EndCutsceneAfterDuration(double duration)
    {
        yield return new WaitForSeconds((float)duration);
        EndCutscene();
    }

   public void EndCutscene()
    {
        cutscenePlayed = true;

        mainCam.gameObject.SetActive(true);
        cutsceneCanvas.gameObject.SetActive(false);
        cineMachineCam.gameObject.SetActive(false); Cursor.lockState = CursorLockMode.Locked;
        LightHouse.gameObject.SetActive(true);
    }

    public void SwitchCam()
    {
        mainCam.gameObject.SetActive(!mainCam.gameObject.activeInHierarchy);
        cutsceneCanvas.gameObject.SetActive(!cutsceneCanvas.gameObject.activeInHierarchy);
        cineMachineCam.SetActive(!cineMachineCam.activeSelf);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("AI") &&cutscenePlayed==true)
        {
            Debug.Log("AI within circle");
        }
    }
}
