using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class CutsceneController : MonoBehaviour
{



   
    public GameObject CinemachineCam;
    public GameObject MainCamera;


    private void Start()
    {
        CinemachineCam.gameObject.SetActive(false);
    }

    void Update()
    {
        if(transform.position.x == 408.0801f)
        {
            SceneManager.LoadScene("GameOverMenu");
        }
    }

  

   public void StartCutscene()
    {
        

        PlayableDirector timeline = GetComponent<PlayableDirector>();
        CinemachineCam.SetActive(true);
        MainCamera.SetActive(false);
        if (timeline != null)
            {
            
            timeline.Play(); // Start playing the timeline

           
        }
            else
            {
                Debug.LogError("Timeline component not found on the TimelineObject.");
            }
       
          
    }

}
