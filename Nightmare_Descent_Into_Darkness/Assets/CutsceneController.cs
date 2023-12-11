using UnityEngine;
using UnityEngine.Playables;

public class CutsceneController : MonoBehaviour
{



   
    public GameObject CinemachineCam;
    public GameObject MainCamera;


    private void Start()
    {
        CinemachineCam.gameObject.SetActive(false);
    }


  

   public void StartCutscene(Vector3 playerPosition)
    {
        CinemachineCam.SetActive(true);
        CinemachineCam.transform.position = MainCamera.transform.position;
        MainCamera.SetActive(false);

        PlayableDirector timeline = GetComponent<PlayableDirector>();
            if (timeline != null)
            {
                timeline.Play(); // Start playing the timeline
                                 // Move the enemy towards the detected player position

            GameManager.Instance.BackToMain();
           
        }
            else
            {
                Debug.LogError("Timeline component not found on the TimelineObject.");
            }
       
          
    }

}
