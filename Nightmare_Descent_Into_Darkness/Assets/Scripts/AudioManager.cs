using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    private AudioSource m_AudioSource;
    public AudioClip AmbientLevelMusic1;
    public AudioClip AmbientLevelMusic2;
    public AudioClip AmbientLevelMusic3;

    
    // Start is called before the first frame update
    void Start()
    {
        AudioClip[] clips = { AmbientLevelMusic1, AmbientLevelMusic2, AmbientLevelMusic3 };
        

        m_AudioSource = GetComponent<AudioSource>();
        
        m_AudioSource.clip = clips[Random.Range(0,3)];
        m_AudioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
