using UnityEngine;
using System.Collections.Generic;

public class SpiderPatrol : MonoBehaviour
{
    public List<Transform> waypoints = new List<Transform>();
    public float speed = 1.0f;
    public float thresholdDistance = 0.1f; // How close it needs to be to the waypoint to consider it "reached"

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
        // Move our position a step closer to the target.
        float step = speed * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, step);

        // Check if we've reached the waypoint
        if (Vector3.Distance(transform.position, targetWaypoint.position) < thresholdDistance)
        {
            // Update the target waypoint
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
            targetWaypoint = waypoints[currentWaypointIndex];
        }
    }
}
