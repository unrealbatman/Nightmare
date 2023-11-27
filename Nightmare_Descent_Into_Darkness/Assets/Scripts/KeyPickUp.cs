using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeyPickUp : MonoBehaviour
{
    public TextMeshProUGUI keyText;
    public TextMeshProUGUI doorText;
    public MeshRenderer key;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            keyText.enabled = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            keyText.enabled = false;
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && keyText.enabled == true)
        {
            keyText.enabled = false;
            key.enabled = false;
            doorText.enabled = !keyText.enabled;
        }
    }
}
