using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class BossCutsceneHandler : MonoBehaviour
{
    public Camera mainCam;
    public GameObject cineMachineCam;
    public PlayableDirector director;
    public Canvas cutsceneCanvas;
    public GameObject LightHouse;
    public Collider sphereCol;
    public Canvas GameplayCanvas;
    bool cutscenePlayed = false;
    public GameObject introText;
    public GameObject outroText;
    public Image blackImage;
    GameObject[] aiObjects;

    private void Start()
    {
         aiObjects= GameObject.FindGameObjectsWithTag("AI");
    }
    void Update()
    {
        if (cutscenePlayed==false && GameManager.Instance.PassedLevels.Count == 1)
        {

            StartCutscene();
        }
       
    }

    void StartCutscene()
    {
        mainCam.gameObject.SetActive(false);
        cutsceneCanvas.gameObject.SetActive(true);
        cineMachineCam.gameObject.SetActive(true);
        GameplayCanvas.gameObject.SetActive(false);
        cutscenePlayed = true;

       
        foreach (GameObject aiObject in aiObjects)
        {
            aiObject.SetActive(false);
        }

        introText.gameObject.SetActive(false);
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
        GameplayCanvas.gameObject.SetActive(true);
        foreach (GameObject aiObject in aiObjects)
        {
            aiObject.SetActive(true);
        }

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
        blackImage.gameObject.SetActive(true);
        foreach (GameObject aiObject in aiObjects)
        {
            aiObject.SetActive(false);
        }
        StartCoroutine(FadeToWhite());
    }

    IEnumerator FadeToWhite()
    {
        for (float i = 0; i <= 1; i += Time.deltaTime)
        {
            blackImage.color = new Color(1, 1, 1, i);
            yield return null;
        }
        outroText.gameObject.SetActive(true);
    }
}
