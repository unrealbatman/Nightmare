using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeCalculator : MonoBehaviour
{
    public float totalTime = 64.03f;
    private float currentTime = 0f;

    void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= totalTime)
        {
            SceneManager.LoadScene("Main");
        }
    }
}
