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

    bool cutscenePlayed = false;

    void Update()
    {
        if (!cutscenePlayed && GameManager.Instance.PassedLevels.Count == 1)
        {
            StartCutscene();
        }
    }

    void StartCutscene()
    {
        cutscenePlayed = true;
        SwitchCam();
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
        SwitchCam();
        Cursor.lockState = CursorLockMode.Locked;
        LightHouse.gameObject.SetActive(true);
    }

    public void SwitchCam()
    {
        mainCam.gameObject.SetActive(!mainCam.gameObject.activeInHierarchy);
        cutsceneCanvas.gameObject.SetActive(!cutsceneCanvas.gameObject.activeInHierarchy);
        cineMachineCam.SetActive(!cineMachineCam.activeSelf);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("AI"))
        {
            Debug.Log("AI within circle");
        }
    }
}
