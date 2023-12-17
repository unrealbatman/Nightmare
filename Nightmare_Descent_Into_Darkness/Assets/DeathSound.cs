using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathSound : MonoBehaviour
{

    public AudioSource sound;
    private void OnEnable()
    {
        AIController.OnPlayerDie += play;
    }

    private void OnDisable()
    {
        AIController.OnPlayerDie -= play;
    }
    private void play()
    {
        if(sound.isPlaying)
        {
            return;
        }
        sound.Play();
    }
}
