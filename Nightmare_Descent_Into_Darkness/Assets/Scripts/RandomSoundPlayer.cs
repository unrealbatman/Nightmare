using UnityEngine;

public class RandomSoundPlayer : MonoBehaviour
{
    public Transform playerTransform;
    public Transform ghostTransform;
    public AudioSource audioSource; // Assign this in the inspector

    public float minTime = 10.0f; // Minimum time between sound plays
    public float maxTime = 30.0f; // Maximum time between sound plays
    public float maxDistance = 30.0f;

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

        AdjustVolumeBasedOnDistance();
    }

    private void AdjustVolumeBasedOnDistance()
    {
        float distance = Vector3.Distance(playerTransform.position, ghostTransform.position);
        float volume = 1.0f - Mathf.Clamp01(distance / maxDistance);
        audioSource.volume = volume;
    }
}
