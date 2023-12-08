using TMPro;
using UnityEngine;
using static TriggerDoorOpen;

public class DoorManager_level2 : MonoBehaviour
{
    public AnimationCurve openSpeedCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0, 1, 0, 0), new Keyframe(0.8f, 1, 0, 0), new Keyframe(1, 0, 0, 0) });
    public float openSpeedMultiplier = 2.0f;
    public float doorOpenAngle = 90.0f;
    public float closeDelay = 2.0f; // Delay before closing the door after everything exits its trigger zone
    public GameObject doorInteractText;
    bool open = false;
    bool enter = false;
    bool hasEntered = false;
    int entityInsideCount = 0; // Counter to keep track of the number of entities (AI or player) inside the trigger zone

    float defaultRotationAngle;
    float currentRotationAngle;
    float openTime = 0;
    [SerializeField]
    private AudioSource audioSource;

    private void OnEnable()
    {
        TriggerDoorOpen.triggerDoorOpenDelegate += OpenDoor;
    }

    private void OnDisable()
    {
        TriggerDoorOpen.triggerDoorOpenDelegate -= OpenDoor;
    }
    void Start()
    {
        defaultRotationAngle = transform.localEulerAngles.y;
        currentRotationAngle = transform.localEulerAngles.y;
    }

    void Update()
    {
        if (openTime < 1)
        {
            openTime += Time.deltaTime * openSpeedMultiplier * openSpeedCurve.Evaluate(openTime);
        }
        transform.localEulerAngles = new Vector3(
            transform.localEulerAngles.x,
            Mathf.LerpAngle(currentRotationAngle, defaultRotationAngle + (open ? doorOpenAngle : 0), openTime),
            transform.localEulerAngles.z
        );

     
        if (enter)
        {
            audioSource.PlayOneShot(audioSource.clip);
            OpenDoor();
        }
       
       
    }




    void CloseDoor()
    {
        if (entityInsideCount <= 0)
        {
            open = false;
            currentRotationAngle = transform.localEulerAngles.y;
            openTime = 0;
            hasEntered = false;
        }
    }

    void OpenDoor()
    {
        open = !open;
        currentRotationAngle = transform.localEulerAngles.y;
        openTime = 0;

        if (!hasEntered)
        {
            hasEntered = true;
            Invoke("CloseDoor", closeDelay);
        }
    }
}






   


 


    

