using UnityEngine;
using UnityEngine.InputSystem;

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
    }

    private void OnDestroy()
    {
        if (triggerDoor != null)
        {
            triggerDoor.OnDoorStateChanged -= HandleDoorStateChanged;
        }
    }

    private void Update()
    {
        // Check if the door is open and 'E' key is pressed
        if (canDoorOpen && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Door is open. E key pressed.");
            GameManager.Instance.BackToMain();
        }
    }

    private void HandleDoorStateChanged(bool isOpen)
    {
        // Update the door state flag based on the event
        canDoorOpen = isOpen;
    }
}
