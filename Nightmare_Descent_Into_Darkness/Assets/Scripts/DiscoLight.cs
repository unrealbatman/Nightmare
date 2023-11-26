using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightRotate : MonoBehaviour
{
    Light lt;
    public float colorChangeInterval = 2.0f; // Time interval between color changes
    public float intensityChangeSpeed = 0.5f; // Speed of intensity change

    public Color[] colors = new Color[4]; // Array of colors to lerp between
    int currentColorIndex = 0;
    float colorChangeTimer = 0.0f;

    void Start()
    {
        lt = GetComponent<Light>();
        lt.color = colors[currentColorIndex]; // Set initial color
    }

    void Update()
    {
        

        // Change color at specified interval
        colorChangeTimer += Time.deltaTime;
        if (colorChangeTimer >= colorChangeInterval)
        {
            colorChangeTimer = 0.0f;

            // Update to the next color in the array
            currentColorIndex = (currentColorIndex + 1) % colors.Length;
        }

        // Lerp color and intensity
        lt.color = Color.Lerp(lt.color, colors[currentColorIndex], Time.deltaTime * intensityChangeSpeed);
        lt.intensity = Mathf.PingPong(Time.time, 2) + 3; // Change intensity between 1 and 3 in a ping-pong manner
    }
}
