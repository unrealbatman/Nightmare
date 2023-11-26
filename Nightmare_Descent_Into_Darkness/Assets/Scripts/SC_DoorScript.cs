using TMPro;
using UnityEngine;

public class SC_DoorScript : MonoBehaviour
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
    bool isPlayer=false;
    private AudioSource audioSource;

    void Start()
    {
        doorInteractText.gameObject.SetActive(false);
        defaultRotationAngle = transform.localEulerAngles.y;
        currentRotationAngle = transform.localEulerAngles.y;
        audioSource = GetComponent<AudioSource>();
        GetComponent<SphereCollider>().isTrigger = true;
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

     
        if (enter &&!isPlayer)
        {
            OpenDoor();
        }
        else if (enter && isPlayer)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                OpenDoor();
                audioSource.PlayOneShot(audioSource.clip);

            }

        }
       
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("AI") )
        {
            enter = true;
            isPlayer = false;
            HandleDoorForward(other);
            audioSource.PlayOneShot(audioSource.clip);

        }
        if (other.CompareTag("Player"))
        {
            enter = true;
            isPlayer = true;
            doorInteractText.gameObject.SetActive(true);
            HandleDoorForward(other);

        }
    }

    private void HandleDoorForward(Collider other)
    {
        entityInsideCount++; // Increment the counter

        Vector3 doorForward = transform.forward;
        Vector3 entityToDoor = other.transform.position - transform.position;
        float dotProduct = Vector3.Dot(doorForward, entityToDoor);

        if (dotProduct > 0)
        {
            doorOpenAngle = -Mathf.Abs(doorOpenAngle);
        }
        else
        {
            doorOpenAngle = Mathf.Abs(doorOpenAngle);
        }

    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("AI")||other.CompareTag("Player")) 
        {
            doorInteractText.gameObject.SetActive(false);
            entityInsideCount--; // Decrement the counter
            if (entityInsideCount <= 0)
            {
                enter = false;
                isPlayer = false;
                entityInsideCount = 0; // Reset the counter
                CloseDoor();
            }
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






   


 


    

