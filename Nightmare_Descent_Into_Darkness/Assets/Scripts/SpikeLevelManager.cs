using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// This script is for controlling the process of level2
/// </summary>
public class SpikeLevelManager : MonoBehaviour
{
    //lights in order
    [SerializeField]
    private List<GameObject> _roadLights;
    [SerializeField]
    private List<GameObject> _leftWallLights;
    [SerializeField]
    private List<GameObject> _rightWallLight;

    public GameObject firstPersonController;
    public AudioClip lightOpeningAudio;

    public static SpikeLevelManager Instance { get; private set; }


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        // public static instance exposed
        Instance = this;


    }
    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject light in _roadLights)
        {
            light.GetComponent<Light>().enabled = false;
            light.GetComponent<AudioSource>().loop = false;
            light.GetComponent<AudioSource>().playOnAwake = false;
        }
        foreach (GameObject light in _leftWallLights)
        {
            light.GetComponent<Light>().enabled = false;
            light.GetComponent<AudioSource>().loop = false;
            light.GetComponent<AudioSource>().playOnAwake = false;
        }
        foreach (GameObject light in _rightWallLight)
        {
            light.GetComponent<Light>().enabled = false;
            light.GetComponent<AudioSource>().loop = false;
            light.GetComponent<AudioSource>().playOnAwake = false;
        }

        LockMove();
        StartCoroutine(Overture());
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LockMove()
    {
        firstPersonController.GetComponent<FirstPersonController>().CanMove = false;
    }

    public void ReleaseMove()
    {
        firstPersonController.GetComponent<FirstPersonController>().CanMove = true;
    }
    IEnumerator Overture() { 
        yield return new WaitForSeconds(2f);
        yield return Opening();
    }

    IEnumerator Opening()
    {
        void turnOnLight(GameObject light)
        {
            light.GetComponent<Light>().enabled = true;
            AudioSource source = light.GetComponent<AudioSource>();
            source.clip = lightOpeningAudio;
            source.Play();
            
        }
        //OpenLights
        float speedDecay = 0.05f;
        float interval = 1f;
        for( int i = 0; i < _leftWallLights.Count; i++)
        {
            if(i == _leftWallLights.Count - 1)
            {
                turnOnLight(_leftWallLights[i]);
                yield return new WaitForSeconds(interval);
                interval -= speedDecay;

                

                turnOnLight(_rightWallLight[i]);
                yield return new WaitForSeconds(interval);
                interval -= speedDecay;

                turnOnLight(_roadLights[i]);
                yield return new WaitForSeconds(interval);
                interval -= speedDecay;
                break;
            }
            if (i % 2 == 0)
            {
        
                turnOnLight(_leftWallLights[i] );
                yield return new WaitForSeconds(interval);
                interval -= speedDecay;

                turnOnLight(_roadLights[i]);
                yield return new WaitForSeconds(interval);
                interval -= speedDecay;

                turnOnLight(_rightWallLight[i]);
                yield return new WaitForSeconds(interval);
                interval -= speedDecay;
            }
            else
            {
                turnOnLight(_rightWallLight[i]);
                yield return new WaitForSeconds(interval);
                interval -= speedDecay;

                turnOnLight(_roadLights[i]);
                yield return new WaitForSeconds(interval);
                interval -= speedDecay;

                turnOnLight(_leftWallLights[i]);
                yield return new WaitForSeconds(interval);
                interval -= speedDecay;


            }
        }
        yield return new WaitForSeconds(1f);
        turnOnLight(_roadLights[_leftWallLights.Count - 1]);
            

           
        
        //Release Move
        ReleaseMove();

    }
}
