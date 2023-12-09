using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EnemyBehavior : MonoBehaviour
{
    public GameObject player; 
    public GameObject enemy; 
    public Animator anim;
    public AudioSource wraith;
    public GameObject key;
    public TextMeshProUGUI findKeyText;

    public float detectionRange = 2f;
    public float findKeyTextRange = 5f;

    void Update()
    {
        if (player == null || enemy == null || anim == null) 
            return;

        float distance = Vector3.Distance(player.transform.position, enemy.transform.position);
        if(distance <= findKeyTextRange && distance > detectionRange)
        {
            findKeyText.enabled = true;
        }
        else
        {
            findKeyText.enabled = false;
        }
        if(distance <= detectionRange)
        {
            findKeyText.enabled = false;
            anim.SetTrigger("attack");
            wraith.PlayOneShot(wraith.clip);
            key.SetActive(true);
        }
        if(PlayerPrefs.GetInt("Level1key") == 1)
        {
            findKeyText.enabled = false;
        }
    }
}
