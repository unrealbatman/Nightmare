using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{

    private enum _AiState { Patrol,Chase,Search
            //,Attack
            }

    [SerializeField]
    private _AiState _currentState;
   
    //private WaitForSeconds _attackTime = new WaitForSeconds(3.5f);
    private WaitForSeconds _searchTime = new WaitForSeconds(3.8f);

    //private bool _isAttacking = false;
    private bool _isSearching = false;
    private bool _isChasing = false;
    
    [SerializeField]
    private GameObject[] _waypoints;
    private Animator _animator;
    private NavMeshAgent _agent;

    private const string _chaseRoutine = "chaseRoutine";
    private const string _searchRoutine = "searchRoutine";

    //private const string _attackRoutine = "AttackRoutine";

    //private const string _attackAnim = "isAttacking";
    private const string _chaseAnim = "isChasing";
    private const string _searchAnim = "isSearching";
    private const string _walkAnim = "isWalking";



    private int _initRandomWaypoint;
    private int _nextWaypoint;
    private bool _isWaypointReversed = false;


    // Start is called before the first frame update
    void Start()
    {
        
        _animator = GetComponent<Animator>();

        if(_animator == null ) {
            Debug.Log("THe animator on the agent is NULL");
        }

        _waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
        foreach (GameObject obj in _waypoints)
        {
            if (obj != null)
            {
                GetComponent<GameObject>();
            }
        }
        _agent = GetComponent<NavMeshAgent>();
         _initRandomWaypoint  = Random.Range(0, _waypoints.Length);
        _currentState = _AiState.Patrol;
        Debug.Log("RANDOM: " + _initRandomWaypoint);
        _agent.SetDestination(_waypoints[1].transform.position);
       // _agent.destination = ;
        _animator.SetBool(_walkAnim, true);

    }

    // Update is called once per frame
    void Update()
    {
        InputControls();
        //AiState();

        /*if (_agent.remainingDistance > 0.5)
        {
            _agent.isStopped =true;
        }
        Debug.Log("Destination: "+_agent.destination);*/
    }

    private void AiState()
    {
        switch (_currentState)
        {
            case _AiState.Patrol:
                //Patrol behavior goes here
                Debug.Log("Patrolling");
                Patrol();
                break;
           /* case _AiState.Chase:
                //walking behavior goes here
                Debug.Log("Chasing");
                //CalculateAiMovement();
                break;
           // case _AiState.Attack:
                //attacking behavior goes here
               *//* Debug.Log("Attacking");
                Attack();
                break;*//*
            case _AiState.Search:
                //jumping behavior goes here
                Debug.Log("Searching");
               // Searching();
                break;*/
            default:
                Debug.LogError("There is no state for this case");
                break;
        }
    }


    private void InputControls()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
           // _currentState = _AiState.Chase;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            /*_isChasing = false;
            _isSearching = false;
            _currentState = _AiState.Patrol;
            _agent.isStopped = false;
            _animator.SetBool(_walkAnim, true);*/
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
           // _currentState = _AiState.Attack;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
           /* _isSearching =true;
            _currentState = _AiState.Search;*/

        }

    }

   private void Patrol()
    {
        Debug.Log("Agent distance to waypoint is " + _agent.remainingDistance);

        if (_agent.remainingDistance < 0.1f)
        {
           StartCoroutine( ReverseWaypoint());
        }
    }


    private IEnumerator ReverseWaypoint()
    {
    

        yield return new WaitForSeconds(5f);
        if (_isWaypointReversed == false)
        {
            if (_nextWaypoint < _waypoints.Length - 1)
            {
                _nextWaypoint++;
            }
            else
            {
                _isWaypointReversed = true;
                _nextWaypoint--;
            }
        }

        else if (_isWaypointReversed)
        {
            if (_nextWaypoint > 0)
            {
                _nextWaypoint--;
            }
            else
            {
                _isWaypointReversed = false;
                _nextWaypoint++;
            }
        }
        _agent.SetDestination (_waypoints[_nextWaypoint].transform.position);
    }



    private void Attack()
    {
        /*if (_isAttacking == false)
        {
            _isAttacking = true;
            StartCoroutine(_attackRoutine);
        }*/
    }


}
