using UnityEngine;

public class SC_DoorScript : MonoBehaviour
{
    public AnimationCurve openSpeedCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0, 1, 0, 0), new Keyframe(0.8f, 1, 0, 0), new Keyframe(1, 0, 0, 0) });
    public float openSpeedMultiplier = 2.0f;
    public float doorOpenAngle = 90.0f;

    bool open = false;
    bool enter = false;
    bool hasEntered = false;
    int aiInsideCount = 0; // Counter to keep track of the number of AI inside

    float defaultRotationAngle;
    float currentRotationAngle;
    float openTime = 0;

    public float closeDelay = 2.0f;

    void Start()
    {
        defaultRotationAngle = transform.localEulerAngles.y;
        currentRotationAngle = transform.localEulerAngles.y;

        GetComponent<SphereCollider>().isTrigger = true;
    }

    void Update()
    {
        if (openTime < 1)
        {
            openTime += Time.deltaTime * openSpeedMultiplier * openSpeedCurve.Evaluate(openTime);
        }
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, Mathf.LerpAngle(currentRotationAngle, defaultRotationAngle + (open ? doorOpenAngle : 0), openTime), transform.localEulerAngles.z);

        if (enter)
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

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("AI"))
        {
            aiInsideCount++; // Increment the counter
            enter = true;

            // Calculate the relative direction of the AI to the door
            Vector3 doorForward = transform.forward;
            Vector3 aiToDoor = other.transform.position - transform.position;
            float dotProduct = Vector3.Dot(doorForward, aiToDoor);

            // Apply opposite rotation based on the AI's position relative to the door
            if (dotProduct > 0) // AI is in front of the door
            {
                doorOpenAngle = -Mathf.Abs(doorOpenAngle); // Open inward
            }
            else // AI is behind the door
            {
                doorOpenAngle = Mathf.Abs(doorOpenAngle); // Open outward
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("AI"))
        {
            aiInsideCount--; // Decrement the counter
            if (aiInsideCount <= 0)
            {
                enter = false;
                aiInsideCount = 0; // Reset the counter
            }
        }
    }

    void CloseDoor()
    {
        if (aiInsideCount <= 0) // Check if there are no AI inside before closing
        {
            open = false;
            currentRotationAngle = transform.localEulerAngles.y;
            openTime = 0;
            hasEntered = false;
        }
    }
}
