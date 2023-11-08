using UnityEngine;

public class EnemyHeadTracking : MonoBehaviour
{
    public Transform player; // Assign the player's transform in the inspector
    public Transform headBone; // Assign the head bone of the enemy in the inspector

    public float rotationSpeed = 5.0f; // Speed at which the head turns

    void Update()
    {
        if (player == null || headBone == null)
            return;

        // Determine which direction to rotate towards
        Vector3 targetDirection = player.position - headBone.position;

        targetDirection.y = 0;

        // The step size is equal to speed times frame time.
        float singleStep = rotationSpeed * Time.deltaTime;

        // Rotate the forward vector towards the target direction by one step
        Vector3 newDirection = Vector3.RotateTowards(headBone.forward, targetDirection, singleStep, 0.0f);

        // Calculate a rotation a step closer to the target and applies rotation to this object
        headBone.rotation = Quaternion.LookRotation(newDirection);
    }
}
