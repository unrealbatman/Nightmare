using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDoorOpen : MonoBehaviour
{
    public delegate void TriggerDoorOpenDelegate();

    public static event TriggerDoorOpenDelegate triggerDoorOpenDelegate;
    private void OnTriggerEnter(Collider other)
    {
        triggerDoorOpenDelegate?.Invoke();
    }
}
