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
    public TextMeshProUGUI whereIsKeyText;
    public KeyPickUp keyPickUp;

    public float detectionRange = 2f;
    public float whereIsKeyTextRange = 5f;

    void Update()
    {
        if (player == null || enemy == null || anim == null) 
            return;

        float distance = Vector3.Distance(player.transform.position, enemy.transform.position);
        if(distance <= whereIsKeyTextRange && distance > detectionRange)
        {
            whereIsKeyText.enabled = true;
        }
        else
        {
            whereIsKeyText.enabled = false;
        }
        if(distance <= detectionRange)
        {
            whereIsKeyText.enabled = false;
            anim.SetTrigger("attack");
            wraith.PlayOneShot(wraith.clip);
            key.SetActive(true);
        }
        if(keyPickUp.key.enabled == false)
        {
            whereIsKeyText.enabled = false;
        }
    }
}
