using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    // Define the possible states
    public enum State
    {
        Patrol,
        Chase,
        Search,
        Attack
    }

    // Current state of the agent
    private State currentState;

    // Variables for state behavior
    private Transform player; // Assuming player is a GameObject with a transform
    private float searchTimer = 0f;
    private float searchDuration = 120f; // 2 minutes for example

    private NavMeshAgent agent;
    [SerializeField]
    private GameObject[] Waypoints;
    private int initRandomWaypoint;
    private int currentWaypointIndex;
    private int patrolDirection=1;// 1 for ascending, -1 for descending

    void Start()
    {

        agent = GetComponent<NavMeshAgent>();
        // Initialize with initial state
        currentState = State.Patrol;
        Waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
        foreach(GameObject go in Waypoints)
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
        // FSM logic
        switch (currentState)
        {
            case State.Patrol:
                PatrolUpdate();
                break;
            case State.Chase:
                ChaseUpdate();
                break;
            case State.Search:
                SearchUpdate();
                break;
            case State.Attack:
                AttackUpdate();
                break;
        }
    }

    void PatrolUpdate()
    {


        if (agent.remainingDistance < 0.2f)
        {
            currentWaypointIndex += patrolDirection;

            // Check if we've reached the end of the array
            if (currentWaypointIndex>=Waypoints.Length || currentWaypointIndex < 0)
            {
                // Change direction
                patrolDirection *= -1;

                // Set the index to the next waypoint based on the new direction
                currentWaypointIndex += patrolDirection; // Move by 2 to prevent immediately turning around
            }
        }
        agent.destination = Waypoints[currentWaypointIndex].transform.position;
  
    }

    void ChaseUpdate()
    {
        // Add behavior for Chase state here
        // Example: Move towards player
        // You can use Vector3.MoveTowards or other methods to approach the player
    }

    void SearchUpdate()
    {
        // Add behavior for Search state here
        // Example: Look for the player's last known position

        // Increment search timer
        searchTimer += Time.deltaTime;

        if (searchTimer >= searchDuration)
        {
            // If search duration is reached, transition to Attack state
            currentState = State.Attack;
            searchTimer = 0f;
        }
    }

    void AttackUpdate()
    {
        // Add behavior for Attack state here
        // Example: Engage in combat with the player
    }

    // Function to transition to Chase state (called when player is detected)
    public void TransitionToChase()
    {
        currentState = State.Chase;
    }

    // Function to transition to Search state (called when player is not visible)
    public void TransitionToSearch()
    {
        currentState = State.Search;
    }
}
