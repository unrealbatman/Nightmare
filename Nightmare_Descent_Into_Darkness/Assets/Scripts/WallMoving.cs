using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WallMoving : MonoBehaviour
{
    public GameObject wall;
    public Vector3 direction;
    public float speed = 0.01f;

    [SerializeField]
    private bool moving = false;
    
    private AudioSource[] sources;
    private Coroutine becomingFaster;
    
    private void Awake()
    {
        sources = GetComponents<AudioSource>();
        SpikeLevelTrigger.OnLevelTrigger += StartMove;
            
    }
    private void OnEnable()
    {


    }

    private void OnDisable()
    {
        SpikeLevelTrigger.OnLevelTrigger -= StartMove;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            gameObject.transform.Translate(direction.normalized * speed, Space.World);

            // ash particle can be added


            // camera shake

            

        }
    }
    public void StartMove()
    {
        if(moving)
        {
            return;
        }
        moving = true;
        // sound effect
        sources[0].Play();
        sources[0].loop = true;

        // ash particle can be added



        becomingFaster = StartCoroutine(SecondPhrase());


    }

    public void StopMove()
    {
        moving = false;
        foreach (var source in sources)
        {
            source.Stop();
        }
        if (becomingFaster != null)
        {
            StopCoroutine(becomingFaster);
            becomingFaster = null;
        }
        
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    IEnumerator SecondPhrase()
    {
        yield return new WaitForSeconds(3f);
        SetSpeed(speed+0.0015f);
        sources[1].Play();
    }
}
