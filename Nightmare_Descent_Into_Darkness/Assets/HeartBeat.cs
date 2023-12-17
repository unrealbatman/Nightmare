using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class HeartBeat : MonoBehaviour
{
    public AudioSource heartBeatSound;
    public CinemachineVirtualCamera camera;
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
        camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 3f;
    }
    public void stopBeat()
    {
        heartBeatSound.Stop();
        heartBeatSound.loop = false;
        camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 0.3f;
    }
}
