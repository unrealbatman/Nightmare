using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;

public class ReceiveSignal : MonoBehaviour
{
    public SignalAsset gameover;

    public UnityEvent toFailure;
    // Start is called before the first frame update
    void Start()
    {
        toFailure.AddListener( goToGameOver);
        SignalReceiver signalReceiver = GetComponent<SignalReceiver>();
        signalReceiver.AddReaction(gameover, toFailure);
    }

    // Update is called once per frame
    public  void goToGameOver() 
    {
        SceneManager.LoadScene("GameOverMenu");
    }
}
