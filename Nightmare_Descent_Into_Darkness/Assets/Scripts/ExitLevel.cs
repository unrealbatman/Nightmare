using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitLevel : MonoBehaviour
{
    public GameObject notePanel;

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
    }
}
