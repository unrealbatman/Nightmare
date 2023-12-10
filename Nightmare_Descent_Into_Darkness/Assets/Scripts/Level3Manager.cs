using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using static KeyPickUp;

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

    public GameObject exitDoor;
    private CheckTriggerDoor triggerDoor;
    private bool canDoorOpen = false; // Flag indicating the door state
    public int numKeysCollected = 0;
    public TextMeshProUGUI exitMansionText;
    public GameObject notePanel;

    void Start()
    {
        InitializeMaterialSettings();
        triggerDoor = exitDoor.GetComponent<CheckTriggerDoor>();
        if (triggerDoor != null)
        {
            triggerDoor.OnDoorStateChanged += HandleDoorStateChanged;
        }
        else
        {
            Debug.LogError("CheckTriggerDoor component not found on exitDoor.");
        }

        // Subscribe to the event from KeyPickUp script
        KeyPickUp.OnKeyPickedUp += KeyCollected;
    }


    private void Update()
    {
        if (canDoorOpen && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Door is open. E key pressed.");
            GameManager.Instance.BackToMain();
        }

    }


    private void HandleDoorStateChanged(bool isOpen)
    {
        canDoorOpen = isOpen;
    }

    private void OnDestroy()
    {
        if (triggerDoor != null)
        {
            triggerDoor.OnDoorStateChanged -= HandleDoorStateChanged;
        }

        // Unsubscribe from the KeyPickUp event
        KeyPickUp.OnKeyPickedUp -= KeyCollected;
    }
    public void KeyCollected()
    {
        numKeysCollected++;
        Debug.Log("Number of keys collected: " + numKeysCollected);
        if (numKeysCollected >= 5)
        {
            PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "Key", 1);
            PlayerPrefs.Save();
            triggerDoor.open = true;
            Debug.Log("All keys collected for Level3! PlayerPrefs set for " + SceneManager.GetActiveScene().name + "Key to 1.");
            DisplayNote();
//            OnKeyPickedUp?.Invoke();
            // You might want to disable further key collection logic here or trigger an event for Level3 completion
            
        }
    }

    public void DisplayNote()
    {
        notePanel.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
    }

    public void CloseNote()
    {
        notePanel.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        exitMansionText.enabled = true;
        StartCoroutine(DisableText());
    }

    IEnumerator DisableText()
    {
        yield return new WaitForSeconds(5f);
        exitMansionText.enabled = false;
    }

   


    #region ColourChange
    void InitializeMaterialSettings()
    {
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
            Color startColor = settings.startColor;
            Color endColor = settings.endColor;
            ColorInterpolationType interpolationType = settings.interpolationType;

            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;
                Color interpolatedColor = GetInterpolatedColor(startColor, endColor, interpolationType, t);
                renderer.material.SetColor("_Color", interpolatedColor);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            SwapColors(ref startColor, ref endColor);
        }
    }

    Color GetInterpolatedColor(Color startColor, Color endColor, ColorInterpolationType interpolationType, float t)
    {
        switch (interpolationType)
        {
            case ColorInterpolationType.Linear:
                return Color.Lerp(startColor, endColor, t);
            case ColorInterpolationType.SmoothStep:
                return Color.Lerp(startColor, endColor, Mathf.SmoothStep(0.0f, 1.0f, t));
            case ColorInterpolationType.EaseIn:
                return Color.Lerp(startColor, endColor, t * t);
            case ColorInterpolationType.EaseOut:
                return Color.Lerp(startColor, endColor, 1 - (1 - t) * (1 - t));
            // Add more cases for other interpolation types as needed
            default:
                return Color.Lerp(startColor, endColor, t);
        }
    }

    void SwapColors(ref Color color1, ref Color color2)
    {
        Color temp = color1;
        color1 = color2;
        color2 = temp;
    }

    #endregion
}
