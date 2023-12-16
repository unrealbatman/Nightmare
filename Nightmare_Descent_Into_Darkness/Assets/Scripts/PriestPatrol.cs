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
    public Transform player; // Assuming player is a GameObject with a transform

    private bool playerDetected = false;
    // Reference to the FieldOfView script (Assuming it's attached to the priest)
    private FieldOfView fieldOfView;

    private NavMeshAgent agent;
    [SerializeField]
    private GameObject[] Waypoints;
    private int initRandomWaypoint;
    private int currentWaypointIndex;



    public float agentMoveSpeed = 0.5f;



    public AudioSource footStepSource;
    public AudioSource voiceSource;

    public AudioClip zombieVoice1;
    public AudioClip zombieVoice2;
    public AudioClip zombieVoice3;
    private AudioClip[] voices;

private Animator animator;
    [SerializeField]
    private float _waitTime = 50f; // in seconds
    private float _waitCounter = 0f;
    private bool _waiting = false;

    void Start()
    {
        voices = new AudioClip[3];
        voices[0] = zombieVoice1;
        voices[1] = zombieVoice2;
        voices[2] = zombieVoice3;
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
        fieldOfView = GetComponent<FieldOfView>();

        StartCoroutine(PlayAudioEveryTenSeconds());

    }
    IEnumerator PlayAudioEveryTenSeconds()
    {
        while (true)
        {
            yield return new WaitForSeconds(8f); // Wait for 10 seconds

            // Play the audio clip
            
                if(!voiceSource.isPlaying) {

                voiceSource.clip = voices[(int)Random.Range(0, 3)];
                    voiceSource.Play();
                }
              
            
        }
    }
    void Update()
    {

        if (playerDetected)
        {
            MoveTowardsPlayer();
        }
        else
        {
            PatrolUpdate();
        }
    }

    void MoveTowardsPlayer()
    {
        if (player == null)
            return;

        agent.SetDestination(player.position);

        // Calculate the direction from the agent to the player
        Vector3 lookDirection = (player.position - transform.position).normalized;
        lookDirection.y = 0f; // Ensure the rotation is in the horizontal plane

        // If the direction is not zero, rotate towards the player
        if (lookDirection != Vector3.zero)
        {
            Quaternion newRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 5f);
        }
        // ... (other logic like stopping footsteps or animations)
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
            if (!footStepSource.isPlaying)
            {
                footStepSource.Play();
            }
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
            Quaternion newRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 5f);
        }

        // Check if the player is detected by the FieldOfView
        if (fieldOfView.visibleTargets.Count > 0)
        {
            player = fieldOfView.visibleTargets[0]; // Assuming the first detected target is the player
            playerDetected = true;
        }
        else
        {
            playerDetected = false;
            player = null;
        }
    }





}


     
 



