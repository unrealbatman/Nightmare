using UnityEngine;
using UnityEngine.UI;

public class ScreenVibration : MonoBehaviour
{
    public RectTransform detectionPanel; // Reference to the UI panel to vibrate
   /* public float panelVibrationAmount = 10.0f; // Amount of UI panel vibration (adjust as needed)
    public float panelVibrationSpeed = 10.0f; // Speed of UI panel vibration (adjust as needed)*/

    public Transform mainCamera; // Reference to the main camera
    public float cameraShakeAmount = 0.1f; // Amount of camera shake (adjust as needed)
    public float cameraShakeSpeed = 50.0f; // Speed of camera shake (adjust as needed)

    public AudioSource heartbeatSound; // Reference to the heartbeat sound

    private Vector3 originalPanelPosition;
    private Vector3 originalCameraPosition;
    private bool isPanelVibrating = false;
    private bool isCameraShaking = false;

    private void Start()
    {
        if (detectionPanel != null)
        {
            originalPanelPosition = detectionPanel.anchoredPosition;
        }

        if (mainCamera != null)
        {
            originalCameraPosition = mainCamera.transform.localPosition;
        }
    }

    private void Update()
    {
        UpdateCameraShake();
    }

   /* private void UpdatePanelVibration()
    {
        if (isPanelVibrating && detectionPanel != null)
        {
            float xOffset = Mathf.Sin(Time.time * panelVibrationSpeed) * panelVibrationAmount;
            float yOffset = Mathf.Cos(Time.time * panelVibrationSpeed) * panelVibrationAmount;

            Vector3 newPosition = originalPanelPosition + new Vector3(xOffset, yOffset, 0);

            detectionPanel.anchoredPosition = newPosition;
        }
    }*/

    private void UpdateCameraShake()
    {
        if (isCameraShaking && mainCamera != null)
        {
            float xOffset = Mathf.Sin(Time.time * cameraShakeSpeed) * cameraShakeAmount;
            float yOffset = Mathf.Cos(Time.time * cameraShakeSpeed) * cameraShakeAmount;

            Vector3 newPosition = originalCameraPosition + new Vector3(xOffset, yOffset, 0);

            mainCamera.transform.localPosition = newPosition;
        }
    }

    public void StartVibration()
    {
        isPanelVibrating = true;
        PlayHeartbeatSound();
        isCameraShaking = true;
    }

    public void StopVibration()
    {
        isPanelVibrating = false;
        detectionPanel.anchoredPosition = originalPanelPosition;
        StopHeartbeatSound();
        isCameraShaking = false;
        mainCamera.transform.localPosition = originalCameraPosition;
    }

    private void PlayHeartbeatSound()
    {
        if (heartbeatSound != null && !heartbeatSound.isPlaying)
        {
            Debug.Log("Heartbeatplauyong");
            heartbeatSound.Play();
        }
    }

    private void StopHeartbeatSound()
    {
        if (heartbeatSound != null && heartbeatSound.isPlaying)
        {
            heartbeatSound.Stop();
        }
    }
}
