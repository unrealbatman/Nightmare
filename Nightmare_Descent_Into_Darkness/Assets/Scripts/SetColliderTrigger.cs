using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetColliderTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SetAllChildCollidersToTrigger(transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetAllChildCollidersToTrigger(Transform parent)
    {
        foreach (Transform child in parent)
        {
            Collider collider = child.GetComponent<Collider>();
            if (collider != null)
            {
                collider.isTrigger = true;
            }


            if (child.childCount > 0)
            {
                SetAllChildCollidersToTrigger(child);
            }
        }
    }
}
