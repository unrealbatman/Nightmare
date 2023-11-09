using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public GameObject player; 
    public GameObject enemy; 
    public Animator anim;
    public AudioSource wraith;

    public float detectionRange = 2f;

    void Update()
    {
        if (player == null || enemy == null || anim == null) 
            return;

        float distance = Vector3.Distance(player.transform.position, enemy.transform.position);
        if(distance <= detectionRange)
        {
            anim.SetTrigger("attack");
            wraith.PlayOneShot(wraith.clip);
        }
    }
}
