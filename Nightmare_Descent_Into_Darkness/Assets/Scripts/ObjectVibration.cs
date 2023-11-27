using UnityEngine;

public class ObjectVibration : MonoBehaviour
{
    [SerializeField]
    public float intensity = 0.1f; // The intensity of the vibration
    public float frequency = 10f; // The frequency of the vibration

    private Rigidbody rb;

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
}
