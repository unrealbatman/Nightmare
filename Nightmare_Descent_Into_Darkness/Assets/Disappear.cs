using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class Disappear : MonoBehaviour
{
    public GameObject hide;
    public GameObject show;
    private bool hasChecked = false;


    private void OnBecameVisible()
    {
        hasChecked = true;
    }

    private void OnBecameInvisible()
    {
        if (hasChecked)
        {
            hide.SetActive(false);
            show.SetActive(true);
        }
    }
}
