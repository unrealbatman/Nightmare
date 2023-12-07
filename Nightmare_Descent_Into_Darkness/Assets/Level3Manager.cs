using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ColorInterpolationType
{
    Linear,
    SmoothStep,
    EaseIn,
    EaseOut
    // Add more interpolation types as needed
}
public class Level3Manager : MonoBehaviour
{
    public GameObject[] materials;

    private Dictionary<Renderer, MaterialSettings> materialToSettingsMap = new Dictionary<Renderer, MaterialSettings>();

    void Start()
    {
        // Store material and its color settings in the dictionary
        for (int i = 0; i < materials.Length; i++)
        {
            Renderer renderer = materials[i].GetComponent<Renderer>();
            MaterialSettings settings = materials[i].GetComponent<MaterialSettings>();
            if (settings != null)
            {
                materialToSettingsMap.Add(renderer, settings);
                StartCoroutine(ColorPingPong(renderer, settings));
            }
            else
            {
                Debug.LogWarning("MaterialSettings missing for material at index " + i);
            }
        }
    }

    IEnumerator ColorPingPong(Renderer renderer, MaterialSettings settings)
    {
        float duration = 2.0f;
        while (true)
        {
            float elapsedTime = 0.0f;
            bool pingPong = false;

            Color startColor = settings.startColor;
            Color endColor = settings.endColor;
            ColorInterpolationType interpolationType = settings.interpolationType;

            while (true) // Continuously loop the color ping-pong
            {
                float t = elapsedTime / duration;

                Color interpolatedColor = new Color(startColor.r, startColor.g, startColor.b, 1.0f);

                switch (interpolationType)
                {
                    case ColorInterpolationType.Linear:
                        interpolatedColor = Color.Lerp(startColor, endColor, t);
                        break;
                    case ColorInterpolationType.SmoothStep:
                        interpolatedColor = Color.Lerp(startColor, endColor, Mathf.SmoothStep(0.0f, 1.0f, t));
                        break;
                    case ColorInterpolationType.EaseIn:
                        interpolatedColor = Color.Lerp(startColor, endColor, t * t);
                        break;
                    case ColorInterpolationType.EaseOut:
                        interpolatedColor = Color.Lerp(startColor, endColor, 1 - (1 - t) * (1 - t));
                        break;
                    // Add more cases for other interpolation types as needed
                    default:
                        break;
                }

                renderer.material.SetColor("_Color", interpolatedColor);

                elapsedTime += Time.deltaTime;

                if (elapsedTime >= duration)
                {
                    elapsedTime = 0.0f;
                    pingPong = !pingPong; // Reverse the ping pong effect
                    Color temp = startColor;
                    startColor = endColor;
                    endColor = temp;
                }

                yield return null;
            }
        }
    }
}