using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

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

    public float detectionRadius = 80f;

    [Range(0, 360)]
    public float fovAngle;
    public float lineOfSightRadius = 180f;

    public AudioSource audioSource;
    public AudioClip patrolClip;
    public AudioClip chaseClip;
    public AudioClip searchClip;
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
                audioSource.clip = patrolClip;
                PatrolUpdate();
                break;
            case State.Chase:
                animator.ResetTrigger("Patrol");
                audioSource.clip = chaseClip;
                ChaseUpdate();
                break;
            case State.Search:
                audioSource.clip = searchClip;
                animator.ResetTrigger("Patrol");
                SearchUpdate();
                break;
            case State.Attack:
                AttackUpdate();
                break;
        }
        if(!audioSource.isPlaying)
        {
            audioSource.Play();
        }


        //Debug.Log("Current State: " + currentState);
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

        if (agent.remainingDistance < 5)
        {
            RaycastHit rayHit;
            if (Physics.Raycast(transform.position, player.transform.position - transform.position, out rayHit, detectionRadius))
            {
                if (rayHit.collider.CompareTag("Player"))
                {
                    agent.speed = 10;
                    Debug.Log("Agfent Remainin distance: " + agent.remainingDistance);
                    if (agent.remainingDistance<=2)
                    {
                        agent.isStopped = true;
                         // Trigger the attack animation here
                         animator.SetTrigger("Attack");
                         currentState = State.Attack;
                        StartCoroutine(loadDelayed());
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

    IEnumerator loadDelayed()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("GameOverMenu");
    }
    void SearchUpdate()
    {

        agent.destination = player.transform.position;

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
    {/*
        // Player is not in attack range, check if player has moved away
        if (Vector3.Distance(transform.position, player.transform.position) >0.1 )
        {
            // Player has moved away, return to Chase state
            currentState = State.Chase;
            agent.isStopped =false;
            animator.ResetTrigger("Attack");
            animator.SetTrigger("Chase");
            Debug.Log("Player has escaped, returning to Chase state.");
            return;
        }
        else
        {

        }*/
    }

    void TransitionLogic()
    {
        if (!canTransition)
        {
            return;
        }

        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, LayerMask.GetMask("SoundMask"));

        bool playerDetected = false;

        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                player = collider.transform;
                playerDetected = true;

                //Debug.Log("I can sense the player");

                if (agent.remainingDistance > 60)
                {
                    if (currentState != State.Chase)
                    {
                        currentState = State.Chase;
                        animator.ResetTrigger("Search");
                        animator.SetTrigger("Chase");
                    }
                    return; // Early exit if the player is far away
                }

                float signedAngle = Vector3.Angle(
                    -transform.forward,
                    player.position - transform.position);

                if (Mathf.Abs(signedAngle) < fovAngle / 2)
                {
                    if (currentState != State.Chase)
                    {
                        currentState = State.Chase;
                        animator.ResetTrigger("Search");
                        animator.SetTrigger("Chase");
                    }
                }
                else
                {
                    //Debug.Log("I cant see the player");
                    currentState = State.Search;
                    animator.ResetTrigger("Chase");
                    animator.SetTrigger("Search");
                }

                return; // Early exit after processing the player
            }
        }

        // If no player is found
        if (!playerDetected && currentState == State.Chase)
        {
            currentState = State.Search;
            animator.ResetTrigger("Chase");
            animator.SetTrigger("Search");
            //Debug.Log("Player exited the detection radius.");
        }
    }



    IEnumerator Cooldown()
        {
            canTransition = false;
            yield return new WaitForSeconds(2f); // Adjust the cooldown duration as needed
            canTransition = true;
        }


#if UNITY_EDITOR
    void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
            Handles.color = new Color(0, 1, 0, 0.3f);
            Handles.DrawSolidArc(transform.position, transform.up, Quaternion.AngleAxis(-fovAngle / 2f,transform.up) * -transform.forward, fovAngle,lineOfSightRadius);
/*            Debug.DrawRay(transform.position, -transform.forward.normalized * lineOfSightRadius, Color.blue, 5f);
*/
        }
#endif

}
