using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeath : MonoBehaviour
{
    GameObject player;
    Animator playerAnimator;
    public int health = 6;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Unity_Icons_Seeker_Rig_var1");
        playerAnimator = player.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < 4f) {
            if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("attack")) {
                this.GetComponent<Rigidbody>().AddForce(player.transform.forward * 2000f);
                health -= 1;
                if(health <= 0)
                {
                    Invoke("Kill", player.transform.forward.magnitude);
                }
            }
        }
    }

    void Kill()
    {
        Destroy(gameObject);
    }
}
