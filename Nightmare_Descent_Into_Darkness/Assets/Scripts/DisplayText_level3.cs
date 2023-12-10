using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DisplayText_level3 : MonoBehaviour
{
    public TextMeshProUGUI findKeysText;

    void Start()
    {
        findKeysText.enabled = true;
        StartCoroutine(DisableText());
    }

    IEnumerator DisableText()
    {
        yield return new WaitForSeconds(5f);
        findKeysText.enabled = false;
    }
}
