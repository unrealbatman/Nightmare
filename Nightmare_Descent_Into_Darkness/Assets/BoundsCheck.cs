using UnityEngine;

public class BoundsCheck : MonoBehaviour
{


 
    private void OnCollisionEnter(Collision other)
    {
        //Debug.Log(other.gameObject.tag);
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("OUTOFBOUNDS");
        }
       
    }
    
  
}
