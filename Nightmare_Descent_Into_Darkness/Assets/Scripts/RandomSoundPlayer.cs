using UnityEngine;

public class RandomSoundPlayer : MonoBehaviour
{
    public AudioSource audioSource; // Assign this in the inspector

    public float minTime = 10.0f; // Minimum time between sound plays
    public float maxTime = 30.0f; // Maximum time between sound plays

    private float timeToNextSound = 0f;

    void Start()
    {
        timeToNextSound = Random.Range(minTime, maxTime);
    }

    void Update()
    {
        if (timeToNextSound <= 0f)
        {
            audioSource.PlayOneShot(audioSource.clip);
            timeToNextSound = Random.Range(minTime, maxTime);
        }
        else
        {
            timeToNextSound -= Time.deltaTime;
        }
    }
}
