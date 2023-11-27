using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthquakeShake : MonoBehaviour
{
    private CinemachineVirtualCamera m_Camera;
    public float intensity = 8f;

    public NoiseSettings mynoisedef;
    private void Awake()
    {
        m_Camera = GetComponent<CinemachineVirtualCamera>();
        if(mynoisedef == null)
        {
            mynoisedef = Resources.Load("Assets/6D Shake.asset") as NoiseSettings;
        }

    }
    private void OnEnable()
    {
        SpikeLevelTrigger.OnLevelTrigger += ShakeCamera;
    }

    private void OnDisable()
    {
        SpikeLevelTrigger.OnLevelTrigger -= ShakeCamera;
    }

    private void ShakeCamera()
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = m_Camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachineBasicMultiChannelPerlin.m_NoiseProfile = mynoisedef;
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
        
        
    }
}
