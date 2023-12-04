using UnityEngine;

public class CheckTriggerDoor : MonoBehaviour
{
    public bool open = false;
    public GameObject doorInteractText;

    public delegate void DoorStateChanged(bool isOpen);
    public event DoorStateChanged OnDoorStateChanged;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && PlayerPrefs.GetInt("Level1key") == 1)
        {
            open = true;
            doorInteractText.SetActive(true);
            OnDoorStateChanged?.Invoke(open);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            open = false;
            doorInteractText.SetActive(false);
            OnDoorStateChanged?.Invoke(open);
        }
    }
}
