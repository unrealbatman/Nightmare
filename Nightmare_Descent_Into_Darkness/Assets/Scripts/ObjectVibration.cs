using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectVibration : MonoBehaviour
{
    [SerializeField]
    public float intensity = 0.1f; // The intensity of the vibration

    private Rigidbody rb;
    public GameObject player;
    public float detectionRange;
    public GameObject Key;
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Call the VibrateObject method repeatedly using InvokeRepeating
        InvokeRepeating("VibrateObject", 0, 0.1f); // Adjust the delay value as needed
    }

    void VibrateObject()
    {
        // Generate random forces along different axes
        float forceX = Random.Range(-intensity, intensity);
        float forceY = Random.Range(-intensity, intensity);
        float forceZ = Random.Range(-intensity, intensity);

        // Apply the forces to the object
        rb.AddForce(new Vector3(forceX, forceY, forceZ), ForceMode.Impulse);
    }

    private void Update()
    {
        DetectPlayer();
    }
    void DetectPlayer()
    {
        float distance = Vector3.Distance(player.transform.position, this.transform.position);
        if (distance <= detectionRange)
        {
            
            Key.SetActive(true);
        }
    }
    
}
