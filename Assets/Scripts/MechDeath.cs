using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MechDeath : MonoBehaviour
{
    public BossHealthBar bar;
    GameObject player;
    Animator playerAnimator;
    public int health = 350;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Unity_Icons_Seeker_Rig_var1");
        playerAnimator = player.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < 6f) {
            if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("attack")) {
                this.GetComponent<Rigidbody>().AddForce(player.transform.forward * 100f);
                health -= 1;
                bar.SetHealth(health);
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
        SceneManager.LoadScene("CreditsMenu");
        Cursor.lockState = CursorLockMode.None;
    }
}
