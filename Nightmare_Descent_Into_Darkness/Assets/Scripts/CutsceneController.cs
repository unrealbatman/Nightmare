    using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class CutsceneController : MonoBehaviour
{



   
    public GameObject CinemachineCam;
    public GameObject MainCamera;

    public AudioSource audioSource1;
    public AudioSource audioSource2;
    public AudioSource audioSource3;
    public AudioSource audioSource4;
    public AudioSource audioSource5;

    private void Start()
    {
       //CinemachineCam.SetActive(false);
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
        audioSource1.Stop();
        audioSource2.Stop();
        audioSource3.Stop();    
        audioSource4.Stop();
        audioSource5.Stop();

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
