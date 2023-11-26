using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WallMoving : MonoBehaviour
{
    public GameObject wall;
    public Vector3 direction;
    public float speed = 0.1f;

    [SerializeField]
    private bool moving = false;
    
    private void Awake()
    {
            
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

            // sound effect
        }
    }
    public void StartMove()
    {
        moving = true;
    }

    public void StopMove()
    {
        moving = false;
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
}
