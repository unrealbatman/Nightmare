using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class lightingingController : MonoBehaviour
{
    public GameObject player;
    public GameObject enemy;
    public Light lighting;
    public Image darknessPanel; // Reference to the CanvasGroup of your UI panel
    public float maxIntensity = 1f;
    public float minIntensity = 0f;
    public float detectionRange = 10f;
    
    // Values for the UI darkness effect
    public float maxDarkness = 0.7f; // Maximum darkness (0 is transparent, 1 is opaque)
    public float minDarkness = 0f; // Minimum darkness (we start fully transparent)

    void Update()
    {
        if (enemy && player && lighting && darknessPanel)
        {
            // Calculate the distance between the player and the enemy
            float distance = Vector3.Distance(player.transform.position, enemy.transform.position);

            // Normalize the distance based on the detection range
            float normalizedDistance = Mathf.Clamp01((distance - detectionRange) / detectionRange);

            // Calculate the new intensity based on the normalized distance
            float intensity = Mathf.Lerp(maxIntensity, minIntensity, 1 - normalizedDistance);
            // Set the lighting intensity
            lighting.intensity = intensity;

            // Calculate the new alpha for the UI panel based on the distance
            float darkness = Mathf.Lerp(minDarkness, maxDarkness, 1 - normalizedDistance);
            // Set the alpha of the darkness panel
            darknessPanel.color = new Color(0, 0, 0, darkness);
        }
    }
}
