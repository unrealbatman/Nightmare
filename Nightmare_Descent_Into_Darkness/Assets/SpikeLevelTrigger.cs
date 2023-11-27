using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeLevelTrigger : MonoBehaviour
{
    public delegate void TriggerEvent();
    public static event TriggerEvent OnLevelTrigger;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnLevelTrigger?.Invoke();
        }
    }
}
