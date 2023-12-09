using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class KeyPickUp : MonoBehaviour
{
    public TextMeshProUGUI keyText;
    public MeshRenderer key;

    public delegate void KeyPickedUp();
    public static event KeyPickedUp OnKeyPickedUp;

    bool isInRange = false;

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
    }
}
