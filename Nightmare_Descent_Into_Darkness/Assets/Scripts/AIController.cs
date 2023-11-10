using System.Collections;
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

    public float agentMoveSpeed = 0.5f;

    public AudioSource playerFootstepAudio;
    public float detectionRadius = 80f;
    public float lineOfSightRadius = 5f;

    private Animator animator;

    private bool canTransition = true; // Flag for transition cooldown

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();

        // Initialize with initial state
        currentState = State.Patrol;
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
        // FSM logic
        switch (currentState)
        {
            case State.Patrol:
                animator.SetTrigger("Patrol");
                PatrolUpdate();
                break;
            case State.Chase:
                animator.ResetTrigger("Patrol");
                ChaseUpdate();
                break;
            case State.Search:
                animator.ResetTrigger("Patrol");
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
                animator.ResetTrigger("Patrol");
                animator.SetTrigger("LookAround");
                return;
            }

            _waiting = false;
        }
        else
        {
            agent.isStopped = false;
            animator.SetTrigger("Patrol");
            animator.ResetTrigger("LookAround");
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

        // Calculate the direction to the target
        Vector3 targetDirection = agent.steeringTarget - transform.position;
        targetDirection.y = 0;

        // If the direction is not zero, rotate towards it
        if (targetDirection != Vector3.zero)
        {
            Quaternion newRotation = Quaternion.LookRotation(-targetDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 5f);
        }
    }

    void ChaseUpdate()
    {
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
            RaycastHit rayHit;
            if (Physics.Raycast(transform.position, player.transform.position - transform.position, out rayHit, detectionRadius))
            {
                if (rayHit.collider.CompareTag("Player"))
                {
                    agent.speed = 10;
                    if (agent.remainingDistance < 0.1)
                    {
                        currentState = State.Attack;
                    }
                    return;
                }
            }
        }

        // Calculate the direction to the target
        Vector3 targetDirection = agent.steeringTarget - transform.position;
        targetDirection.y = 0;

        // If the direction is not zero, rotate towards it
        if (targetDirection != Vector3.zero)
        {
            Quaternion newRotation = Quaternion.LookRotation(-targetDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 5f);
        }
    }

    void SearchUpdate()
    {
        agent.isStopped = true;

        agent.destination = new Vector3(Random.Range(agent.destination.x - 5, agent.destination.x + 5), transform.position.y, Random.Range(agent.destination.z - 5, agent.destination.z + 5));
        agent.isStopped = false;

        searchTimer += Time.deltaTime;

        if (searchTimer >= searchDuration)
        {
            currentState = State.Patrol;
            searchTimer = 0f;
        }

        // Calculate the direction to the target
        Vector3 targetDirection = agent.steeringTarget - transform.position;
        targetDirection.y = 0;

        // If the direction is not zero, rotate towards it
        if (targetDirection != Vector3.zero)
        {
            Quaternion newRotation = Quaternion.LookRotation(-targetDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 5f);
        }
    }

    void AttackUpdate()
    {
        // Add behavior for Attack state here
    }

    void TransitionLogic()
    {
        if (!canTransition)
        {
            return;
        }

        Collider[] collider = Physics.OverlapSphere(transform.position, detectionRadius, LayerMask.GetMask("SoundMask"));

        foreach (var hitCollider in collider)
        {
            if (hitCollider.CompareTag("Player"))
            {
                player = hitCollider.transform;
                currentState = State.Chase;

                animator.SetTrigger("Chase");
                animator.ResetTrigger("Search");

                StartCoroutine(Cooldown());
            }
            else
            {
                animator.SetTrigger("Patrol");
                animator.ResetTrigger("Chase");

                currentState = State.Search;

                StartCoroutine(Cooldown());
            }
        }

        if (currentState == State.Chase)
        {
            RaycastHit raycastHit;
            if (Physics.Raycast(transform.position, transform.forward, out raycastHit, lineOfSightRadius))
            {
                if (!raycastHit.collider.CompareTag("Player"))
                {
                    currentState = State.Search;
                    animator.ResetTrigger("Chase");
                    animator.SetTrigger("Search");
                }
                else
                {
                    currentState = State.Chase;
                    animator.SetTrigger("Chase");
                    animator.ResetTrigger("Search");
                }
            }
        }
    }

    IEnumerator Cooldown()
    {
        canTransition = false;
        yield return new WaitForSeconds(2f); // Adjust the cooldown duration as needed
        canTransition = true;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
