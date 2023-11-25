using System.Collections;
using UnityEngine;
using UnityEngine.AI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PriestPatrol : MonoBehaviour
{



    [SerializeField]
    // Variables for state behavior
    private Transform player; // Assuming player is a GameObject with a transform



    private NavMeshAgent agent;
    [SerializeField]
    private GameObject[] Waypoints;
    private int initRandomWaypoint;
    private int currentWaypointIndex;

   

    public float agentMoveSpeed = 0.5f;


    [Range(0, 360)]
    public float fovAngle;
    public float lineOfSightRadius = 180f;

    public AudioSource audioSource;
    public AudioClip patrolClip;
    public AudioClip footStep;
    private Animator animator;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();

        // Initialize with initial state
        Waypoints = GameObject.FindGameObjectsWithTag("Waypoint");

        foreach (GameObject go in Waypoints)
        {
            if (go != null)
            {
                GetComponent<GameObject>();
            }
        }

        initRandomWaypoint = Random.Range(0, Waypoints.Length);
        currentWaypointIndex = initRandomWaypoint;
        agent.SetDestination(Waypoints[initRandomWaypoint].transform.position);


    }

    void Update()
    {

        PatrolUpdate();
        CheckLineOfSight();
    }

    void PatrolUpdate()
    {
  

        Transform wp = Waypoints[currentWaypointIndex].transform;
        if (!agent.pathPending && agent.remainingDistance < 0.1f)
        {
            // Returns if no points have been set up
            if (Waypoints.Length == 0)
                return;

            transform.position = wp.position;
            

            // Choose the next point in the array as the destination,
            // cycling to the start if necessary.
            currentWaypointIndex = (currentWaypointIndex + 1) % Waypoints.Length;
        }

        // Set the agent to go to the currently selected destination.
        agent.destination = Waypoints[currentWaypointIndex].transform.position;

        // Calculate the direction to the target
        Vector3 targetDirection = agent.steeringTarget - transform.position;
        targetDirection.y = 0;

        // If the direction is not zero, rotate towards it
        if (targetDirection != Vector3.zero)
        {
            Quaternion newRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 5f);
        }
    }


      

 

    void CheckLineOfSight()
    {

    float signedAngle = Vector3.Angle(
        -transform.forward,
        player.position - transform.position);

    if (Mathf.Abs(signedAngle) < fovAngle / 2)
    {
        Debug.Log("I can sense the player");

    }
    else
    {
        //Debug.Log("I cant see the player");
                    
    }

    return; // Early exit after processing the player
    }
        

     
 


#if UNITY_EDITOR
    void OnDrawGizmosSelected()
        {
            Handles.color = new Color(0, 1, 0, 0.3f);
            Handles.DrawSolidArc(transform.position, transform.up, Quaternion.AngleAxis(-fovAngle / 2f,transform.up) * -transform.forward, fovAngle,lineOfSightRadius);
        }
#endif

}
