using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using System.Collections;

public class KeyPickUp : MonoBehaviour
{
    public TextMeshProUGUI keyText;
    public TextMeshProUGUI whereIsKeyText;
    public TextMeshProUGUI exitDoorText;
    public MeshRenderer key;

    public delegate void KeyPickedUp();
    public static event KeyPickedUp OnKeyPickedUp;

    bool isInRange = false;

    void Start()
    {
        whereIsKeyText.enabled = true;
        StartCoroutine(DisableText());
    }

    IEnumerator DisableText()
    {
        yield return new WaitForSeconds(3f);
        whereIsKeyText.enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            
            keyText.enabled = true;
            isInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            keyText.enabled = false;
            isInRange = false;
        }
    }

    void Update()
    {
        if (isInRange && Input.GetKeyDown(KeyCode.E))
        {
            CollectKey();
        }
        key.transform.Rotate(Vector3.up, 5.0f * Time.deltaTime);

    }

    public void CollectKey()
    {
        Debug.Log("here");
        keyText.enabled = false;
        key.enabled = false;
        OnKeyPickedUp?.Invoke();
        key.gameObject.SetActive(false);
        exitDoorText.enabled = true;
        StartCoroutine(DisableExitDoorText());
    }

    IEnumerator DisableExitDoorText()
    {
        yield return new WaitForSeconds(10f);
        exitDoorText.enabled = false;
    }
}
