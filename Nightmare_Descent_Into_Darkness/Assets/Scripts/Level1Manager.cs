using UnityEngine;

public class Level1Manager : MonoBehaviour
{
    public GameObject exitDoor;
    private CheckTriggerDoor triggerDoor;
    private bool canDoorOpen = false; // Flag indicating the door state

    private void Start()
    {
        triggerDoor = exitDoor.GetComponent<CheckTriggerDoor>();
        if (triggerDoor != null)
        {
            triggerDoor.OnDoorStateChanged += HandleDoorStateChanged;
        }
        else
        {
            Debug.LogError("CheckTriggerDoor component not found on exitDoor.");
        }

        // Subscribe to the event from KeyPickUp script
        KeyPickUp.OnKeyPickedUp += HandleKeyPickedUp;
    }

    private void OnDestroy()
    {
        if (triggerDoor != null)
        {
            triggerDoor.OnDoorStateChanged -= HandleDoorStateChanged;
        }

        // Unsubscribe from the KeyPickUp event
        KeyPickUp.OnKeyPickedUp -= HandleKeyPickedUp;
    }

    private void Update()
    {
        if (canDoorOpen && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Door is open. E key pressed.");
            GameManager.Instance.BackToMain();
        }
    }

    private void HandleDoorStateChanged(bool isOpen)
    {
        canDoorOpen = isOpen;
    }

    private void HandleKeyPickedUp()
    {
        // Set PlayerPrefs indicating the key is picked up for Level 1
        PlayerPrefs.SetInt("Level1Key", 1);
        PlayerPrefs.Save();

        // Now handle logic related to the key being picked up
        triggerDoor.open = true;
    }
}
