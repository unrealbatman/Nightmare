using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class HeartBeat : MonoBehaviour
{
    public AudioSource heartBeatSound;
    public CinemachineVirtualCamera ca;
    // Start is called before the first frame update
    private void OnEnable()
    {
        AIController.OnPlayerDetect += beat;
        AIController.OnPlayerHide += stopBeat;

    }

    private void OnDisable()
    {
        AIController.OnPlayerDetect -= beat;
        AIController.OnPlayerHide -= stopBeat;
    }

    public void beat()
    {
        heartBeatSound.Play();
        heartBeatSound.loop = true;
        Debug.Log(heartBeatSound.isPlaying);
        ca.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 2.5f;
    }
    public void stopBeat()
    {
        heartBeatSound.Stop();
        heartBeatSound.loop = false;
        ca.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 0.3f;
    }
}
