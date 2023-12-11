using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Playables;

public class CutsceneHandler : MonoBehaviour
{
     [SerializeField] float fogDensity;
    [SerializeField] Color fogColor;
    [SerializeField] AmbientMode ambientMode;
    PlayableDirector director;
    // Start is called before the first frame update
    void Start()
    {
        director =GetComponent<PlayableDirector>();
    }

    // Update is called once per frame
    void Update()
    {
        RenderSettings.fogDensity = fogDensity;
        RenderSettings.fogColor = fogColor;
        RenderSettings.ambientMode = ambientMode; 
        RenderSettings.ambientMode = ambientMode;
    }
    public void Play(float time)
    {
        director.time = time;
    }
}
