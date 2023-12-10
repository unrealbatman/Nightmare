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
    public GameObject notePanel;

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
            StartCoroutine(ShowNote());
            CollectKey();
        }
        key.transform.Rotate(Vector3.up, 5.0f * Time.deltaTime);

    }

    IEnumerator ShowNote()
    {
        yield return new WaitForSeconds(5f);
        notePanel.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
    }

    public void CloseNote()
    {
        notePanel.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
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
