using UnityEngine;
using System.Collections.Generic;

public class SpiderPatrol : MonoBehaviour
{
    public List<Transform> waypoints = new List<Transform>();
    public float speed = 1.0f;
    public float thresholdDistance = 0.1f; // How close it needs to be to the waypoint to consider it "reached"
    public bool rotateZ = false;
    public bool rotateY = true;

    private int currentWaypointIndex = 0;
    private Transform targetWaypoint;

    void Start()
    {
        if (waypoints.Count > 0)
        {
            targetWaypoint = waypoints[currentWaypointIndex];
        }
    }

    void Update()
    {
        if (targetWaypoint != null)
        {
            MoveTowardsTargetWaypoint();
        }
    }

    private void MoveTowardsTargetWaypoint()
    {
        // Calculate direction to move
        Vector3 direction = targetWaypoint.position - transform.position;
        direction.Normalize();

        // Move the spider
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, step);

        // Calculate rotation only for the forward direction
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        Quaternion forwardRotation;

        if (rotateY)
        {
            forwardRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, lookRotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        }
        else if (rotateZ)
        {
             forwardRotation = Quaternion.Euler(lookRotation.eulerAngles.x, lookRotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        }
        else
        {
            forwardRotation = transform.rotation;
        }

        // Rotate the spider's forward direction
        transform.rotation = Quaternion.Slerp(transform.rotation, forwardRotation, Time.deltaTime * 5f);

        // Check if we've reached the waypoint
        if (Vector3.Distance(transform.position, targetWaypoint.position) < thresholdDistance)
        {
            // Update the target waypoint
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
            targetWaypoint = waypoints[currentWaypointIndex];
        }
    }
}
