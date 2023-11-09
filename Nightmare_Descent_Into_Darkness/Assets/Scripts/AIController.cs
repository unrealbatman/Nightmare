using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

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

    [SerializeField]
    private float searchDuration = 20f; // 2 minutes for example

    private NavMeshAgent agent;
    [SerializeField]
    private GameObject[] Waypoints;
    private int initRandomWaypoint;
    private int currentWaypointIndex;


    [SerializeField]
    private float _waitTime = 50f; // in seconds
    private float _waitCounter = 0f;
    private bool _waiting = false;

    public float agentMoveSpeed =0.5f;


    public AudioSource playerFootstepAudio;
    public float detectionRadius = 80f;
    public float lineOfSightRadius =5f ;
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

        Debug.Log("Current State: " + currentState);
        TransitionLogic();
    }


    



    
    void PatrolUpdate()
    {

        if (_waiting)
        {
            agent.isStopped = true;
            //look around animation
            _waitCounter += Time.deltaTime;
            if (_waitCounter < _waitTime)
            {

                return;
            }
                
            _waiting = false;
        }
        else
        {
            agent.isStopped = false;

        }



        Transform wp = Waypoints[currentWaypointIndex].transform;
        if (!agent.pathPending && agent.remainingDistance < 0.1f)
        {



            // Returns if no points have been set up
            if (Waypoints.Length == 0)
                return;


            transform.position = wp.position;
            _waitCounter = 0f;
            _waiting = true;

            // Choose the next point in the array as the destination,
            // cycling to the start if necessary.
            currentWaypointIndex = (currentWaypointIndex + 1) % Waypoints.Length;
        }
       

        // Set the agent to go to the currently selected destination.
        agent.destination = Waypoints[currentWaypointIndex].transform.position;

    }

    void ChaseUpdate()
    {
        // Add behavior for Chase state here
        // Example: Move towards player
        // You can use Vector3.MoveTowards or other methods to approach the player

        agent.destination = player.transform.position;
        if (Vector3.Distance(transform.position, player.transform.position) > 20)
        {
            agent.speed = 15;
        }
        else 
        {
            agent.speed = 8;    
        }

      
            if (Vector3.Distance(transform.position, player.transform.position) < 5)
        {
            // If a potential player target is detected, perform a raycast to check for line of sight
            RaycastHit rayHit;
            if (Physics.Raycast(transform.position, player.transform.position - transform.position, out rayHit, detectionRadius))
            {
                if (rayHit.collider.CompareTag("Player"))
                {
                    agent.speed = 10;
                    if (agent.remainingDistance < 0.1)
                    {
                        //Attack animaiton plus attack
                    }
                    return;
                }
            }
        }
    }



    void TransitionLogic()
    {
        // Use SphereCast to detect potential targets within the detection radius
        Collider[] collider = Physics.OverlapSphere(transform.position, detectionRadius, LayerMask.GetMask("SoundMask"));

        foreach (var hitCollider in collider)
        {
            if (hitCollider.CompareTag("Player"))
            {

                player = hitCollider.transform;
                currentState = State.Chase;

            }else
            {
                currentState = State.Search;
            }

        }

        if (currentState == State.Chase )
        {
            RaycastHit raycastHit;
            if (Physics.Raycast(transform.position, transform.forward, out raycastHit, lineOfSightRadius))
            {
                if (!raycastHit.collider.CompareTag("Player"))
                {
                    currentState = State.Search;

                }
                else
                {
                    currentState = State.Chase;
                }
            }



        }

    }
    void SearchUpdate()
    {
        // Add behavior for Search state here
        // Example: Look for the player's last known position


        agent.isStopped = true;

        agent.destination = new Vector3(Random.Range(agent.destination.x - 5, agent.destination.x + 5),transform.position.y, Random.Range(agent.destination.z - 5, agent.destination.z + 5));
        agent.isStopped = false;
        // Increment search timer
        searchTimer += Time.deltaTime;

        if (searchTimer >= searchDuration)
        {
            //play search animation

            // If search duration is reached, transition to Attack state
            currentState = State.Patrol;
            searchTimer = 0f;
        }
    }

    void AttackUpdate()
    {
        // Add behavior for Attack state here
        // Example: Engage in combat with the player
    }


    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }



   
}
