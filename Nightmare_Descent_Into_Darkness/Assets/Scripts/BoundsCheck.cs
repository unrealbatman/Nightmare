using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class BoundsCheck : MonoBehaviour
{
    public TextMeshProUGUI   outOfBoundsText;
 
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if (other.CompareTag("Player"))
        {
            outOfBoundsText.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            outOfBoundsText.gameObject.SetActive(false);
        }
    }




}
