using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Manager : MonoBehaviour
{
    bool open=false;
   
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerPrefs.GetInt("Level1key") == 1)
        {

            if (open && PlayerPrefs.GetInt("Level1key")==1 && Input.GetKeyDown(KeyCode.E))
            {

                GameManager.Instance.BackToMain();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            open = true;
        }
        else
        {
            open = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            open = false;
        }
    }
}
