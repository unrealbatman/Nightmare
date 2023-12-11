using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitLevel : MonoBehaviour
{
    public GameObject notePanel;
    public GameObject player;

    void Update()
    {
        if(player.transform.position.y < -20)
        {
            SceneManager.LoadScene("GameOverMenu");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")){
            ExitLevelCoroutine();
        }
    }

    public void ExitLevelCoroutine()
    {
        notePanel.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
       
    }

    public void CloseNote()
    {
        Time.timeScale = 1f;
        GameManager.Instance.BackToMain();
        Cursor.lockState = CursorLockMode.Locked;
        notePanel.SetActive(false);
        GameManager.Instance.LevelFinish("Level2");
    }
}
